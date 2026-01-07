using Dapper;
using OnlineCashBackendApiService.Services;
using OnlineCashTransportModels;

namespace OnlineCashBackendApiService.Handlers;

public static class ManualSupplierQuery
{
    public static async Task<IEnumerable<SupplierResponseTransportModel>> Query(IDbContextFactory contextFactory)
    {
        using var db = contextFactory.CreateDbContext();
        return await db.QueryAsync<SupplierResponseTransportModel>("SELECT * FROM suppliers ORDER BY Name");
    }
}
