using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OnlineCashRmk.Launcher;

internal static class LoadingConfig
{
    public static UpdateConfig LoadConfig()
    {
        var configPath = Path.Combine(Application.StartupPath, "Config.json");
        if (!File.Exists(configPath))
        {
            throw new FileNotFoundException($"Файл конфигурации не найден: {configPath}");
        }

        var json = File.ReadAllText(configPath);
        var config = JsonSerializer.Deserialize<UpdateConfig>(json, new JsonSerializerOptions
        {
            AllowTrailingCommas = true,
            ReadCommentHandling = JsonCommentHandling.Skip,
            PropertyNameCaseInsensitive = true
        });

        if (config == null)
            throw new InvalidOperationException("Не удалось загрузить конфигурацию.");

        // Проверка обязательных полей
        if (string.IsNullOrWhiteSpace(config.VersionUrl))
            throw new InvalidOperationException("В конфигурации отсутствует VersionUrl.");
        if (string.IsNullOrWhiteSpace(config.DownloadUrl))
            throw new InvalidOperationException("В конфигурации отсутствует DownloadUrl.");

        return config;
    }
}
