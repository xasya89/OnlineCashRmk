
using MySql.Data.MySqlClient;

namespace OnlineCashBackendApiService.Services;

public interface IDbContextFactory
{
    MySqlConnection CreateDbContext();
}

public class DbContextFactory : IDbContextFactory
{
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor contextAccessor;

    public DbContextFactory(IHttpContextAccessor accessor, IConfiguration configuration)
    {
        _configuration = configuration;
        contextAccessor = accessor;
    }

    public MySqlConnection CreateDbContext()
    {

        var tenantName = contextAccessor.HttpContext?
            .Request.Headers["X-shopDbName"].FirstOrDefault();

        if (string.IsNullOrWhiteSpace(tenantName))
            throw new InvalidOperationException("Заголовок X-shopDbName не указан.");

        var connectionString = BuildConnectionString(tenantName);
        return new MySqlConnection(connectionString);
    }

    private string BuildConnectionString(string tenant)
    {
        return _configuration.GetConnectionString(tenant);
    }

    private static bool IsValidTenantName(string name)
    {
        return System.Text.RegularExpressions.Regex.IsMatch(name, @"^[a-zA-Z0-9\-_]+$");
    }
}