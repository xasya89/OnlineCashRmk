using Microsoft.Extensions.Caching.Distributed;

namespace OnlineCashBackendApiService.Services;

public class IdempotencyMiddleware(
    RequestDelegate _next,
        IDistributedCache _cache,
        ILogger<IdempotencyMiddleware> _logger
    )
{

    public async Task InvokeAsync(HttpContext context)
    {
        //if(context.Request.Headers.TryGetValue("X-shopDbName", out var _))
        if (context.Request.Method != HttpMethods.Post)
        {
            await _next(context);
            return;
        }
        context.Request.Headers.TryGetValue("X-Document-UUID", out var uuidValues);
        context.Request.Headers.TryGetValue("X-ShopDbName", out var shopDbName);

        var uuid = uuidValues.ToString().Trim();
        var _shopDbName = shopDbName.ToString().Trim();
        if (string.IsNullOrWhiteSpace(uuid) || string.IsNullOrWhiteSpace(_shopDbName))
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/text";
            await context.Response.WriteAsync("shopDbName or Uuid is empty, middleware");
            return;
        }

        var cacheKey = $"idemp:{_shopDbName}:{uuid}";
        if (await _cache.GetStringAsync(cacheKey) == "processed")
        {
            context.Response.StatusCode = 200;
            return;
        }

        // Выполняем запрос
        await _next(context);

        // Если после выполнения статус 2xx — кэшируем
        if (context.Response.StatusCode == 200)
        {
            await _cache.SetStringAsync(cacheKey, "processed",
                new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24) });
        }
    }
}
