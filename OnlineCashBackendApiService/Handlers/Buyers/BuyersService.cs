using System.Collections.Concurrent;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace OnlineCashBackendApiService.Handlers.Buyers;


public class DiscountService
{
    private record Item(bool isBlocked, Guid uuid, string PhoneNumber, string Fio, int SpecialPercent, decimal SumDiscount);
    private readonly string _filePath;
    // Потокобезопасный словарь для хранения в памяти
    private readonly ConcurrentDictionary<string, Item> _cache;
    // Семафор для блокировки операций записи в файл (чтобы не писали одновременно)
    private readonly SemaphoreSlim _fileLock;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly ILogger<DiscountService> _logger;

    public DiscountService(IWebHostEnvironment environment, ILogger<DiscountService> logger)
    {
        _filePath = Path.Combine(environment.ContentRootPath, "discounts.json");
        _cache = new ConcurrentDictionary<string, Item>();
        _fileLock = new SemaphoreSlim(1, 1);

        _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        };
        _logger = logger;

        // Инициализация при создании сервиса (при первом запросе или старте)
        InitializeStorage();
    }

    private void InitializeStorage()
    {
        // Если файл есть - загружаем в память
        if (File.Exists(_filePath))
        {
            LoadFromFileToCache();
        }
        else
        {
            // Если нет - создаем с примером
            var defaultItem = new Item(false, Guid.NewGuid(), "79001001010", "Ghbvth", 10, 0.00m);
            _cache.TryAdd(defaultItem.PhoneNumber, defaultItem);
            SaveCacheToFileAsync().Wait(); // Ждем сохранения при инициализации
        }
    }

    private void LoadFromFileToCache()
    {
        try
        {
            var json = File.ReadAllText(_filePath);
            var items = JsonSerializer.Deserialize<List<Item>>(json, _jsonOptions);

            _cache.Clear();
            if (items != null)
            {
                foreach (var item in items)
                {
                    // Используем PhoneNumber как ключ
                    _cache.TryAdd(item.PhoneNumber, item);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error loading file: {ex.Message}");
        }
    }

    private async Task SaveCacheToFileAsync()
    {
        // Блокируем доступ к файлу для других потоков
        await _fileLock.WaitAsync();
        try
        {
            var items = _cache.Values.ToList();
            var json = JsonSerializer.Serialize(items, _jsonOptions);
            await File.WriteAllTextAsync(_filePath, json);
        }
        finally
        {
            _fileLock.Release();
        }
    }

    public Task<IEnumerable<(bool isBlocked, Guid uuid, string phoneNumber, int specilaPercent)>> GetAllItemsAsync()
    {
        var result = _cache.Values.Select(item =>
            (isBlocked: item.isBlocked, uuid: item.uuid, phoneNumber: item.PhoneNumber, specialPercent: item.SpecialPercent)
        ).ToList();

        return Task.FromResult<IEnumerable<(bool isBlocked, Guid uuid, string phoneNumber, int specialPercent)>>(result);
    }

    public async Task<bool> UpdateDiscountAsync(string phoneNumber, decimal newSumDiscount)
    {
        if (!_cache.TryGetValue(phoneNumber, out var existingItem))
        {
            return false;
        }
        var updatedItem = existingItem with { SumDiscount = existingItem.SumDiscount + newSumDiscount };
        _cache.AddOrUpdate(phoneNumber, updatedItem, (key, oldValue) => updatedItem);
        try
        {
            await SaveCacheToFileAsync();
        }
        catch
        {
            return false;
        };

        return true;
    }
}