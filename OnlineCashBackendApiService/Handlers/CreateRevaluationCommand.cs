using Dapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OnlineCashBackendApiService.Services;
using OnlineCashTransportModels;

namespace OnlineCashBackendApiService.Handlers;

public static class CreateRevaluationCommand
{
    public static async Task<IResult> Handler([FromBody] CreateRevaluationTransportModel body, IDbContextFactory factory)
    {
        using var db = factory.CreateDbContext();
        await db.OpenAsync();
        var tran = await db.BeginTransactionAsync();
        var _sumOld = body.Items.Sum(x=>x.Quantity * x.PriceOld);
        var _sumNew = body.Items.Sum(x => x.Quantity * x.PriceNew);
        var id = await db.QuerySingleAsync<int>(@"INSERT INTO revaluations (`Create`, Status, SumNew, SumOld, Uuid)
            VALUES (@Create, 2, @SumOld, @SumNew, @Uuid);
            SELECT LAST_INSERT_ID();",
            new { Create = body.Create, SumOld=_sumOld, SumNew=_sumNew, Uuid=body.Uuid });

        var uuids = body.Items.Select(x => x.GoodUuid).ToHashSet();
        var idsUuids = (await db.QueryAsync<GoodIdUuid>("SELECT id, uuid FROM Goods WHERE uuid IN @Uuids",
            new { Uuids = uuids })).ToDictionary(x=>x.uuid);

        var sql = "INSERT INTO revaluationgoods (RevaluationId, GoodId, Count, PriceOld, PriceNew) VALUES " +
                      string.Join(",", body.Items.Select((_, i) => $"(@RevaluationId{i}, @GoodId{i}, @Count{i}, @PriceOld{i}, @PriceNew{i})"));
        var p = new DynamicParameters();
        int i = 0;
        foreach(var item in body.Items)
        {
            p.Add($"RevaluationId{i}", id);
            p.Add($"GoodId{i}", idsUuids[item.GoodUuid].id);
            p.Add($"Count{i}", item.Quantity);
            p.Add($"PriceOld{i}", item.PriceOld);
            p.Add($"PriceNew{i}", item.PriceNew);
            i++;
        };
        await db.ExecuteAsync(sql, p);
        await tran.CommitAsync();
        return Results . Ok();
    }

    private record struct GoodIdUuid(int id, Guid uuid);
}
