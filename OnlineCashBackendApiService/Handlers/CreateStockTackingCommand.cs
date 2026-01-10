using Dapper;
using Microsoft.AspNetCore.Mvc;
using OnlineCashBackendApiService.Services;
using OnlineCashTransportModels;
using System.Text.RegularExpressions;

namespace OnlineCashBackendApiService.Handlers;

public static class CreateStockTackingCommand
{
    public static async Task<IResult> Handler([FromBody] StocktackingTransportModel body, IDbContextFactory dbContextFactory)
    {
        using var db = dbContextFactory.CreateDbContext();
        await db.OpenAsync();
        var tran = await db.BeginTransactionAsync();
        var num = await db.QuerySingleAsync<int>("SELECT IFNULL(MAX(Num) + 1, 1) FROM stocktakings");
        var countfact = body.Groups.SelectMany(x => x.Goods).Sum(x => x.CountFact);
        var goodsUuidPrice = (await db.QueryAsync<GoodPrice>("SELECT id, uuid, price FROM goods")).ToDictionary(x=>x.uuid);
        var sumFact = body.Groups.SelectMany(x => x.Goods).Sum(x => x.CountFact * goodsUuidPrice[x.Uuid].price);
        var stocktackingId = await db.QuerySingleAsync<int>(@"INSERT INTO stocktakings
            (Num, Create, ShopId, isSuccess, Status, CountDb, CountFact, SumDb, SumFact, Start, CashMoneyDb, CashMoneyFact, Uuid)
            VALUES
            (@Num, @Create, 1, 1, 2, 0, @CountFact, 0, @SumFact, @Create, 0, @CashMoneyFact, @Uuid)",
            new
            {
                Num=num,
                Create = body.Create,
                CountFact = countfact,
                SumFact=sumFact,
                Start=body.Create,
                CashMoneyFact=body.CashMoney,
                Uuid=body.Uuid,
            });

        foreach (var group in body.Groups)
        {
            var groupId = await db.QuerySingleAsync<int>(@"INSERT INTO stocktakinggroups (StocktakingId, Name) VALUES 
                (@StocktakingId, @Name)",
                new { StocktakingId = stocktackingId, Name = group.Name });


            const int batchSize = 100;

            foreach (var batch in group.Goods.Select((item, index) => new { item, index })
                                       .GroupBy(x => x.index / batchSize)
                                       .Select(g => g.Select(x => x.item).ToArray()))
            {
                var sql = "INSERT INTO stocktakinggoods (StockTakingGroupId, GoodId, Count, CountDB, CountFact, Price) VALUES " +
                          string.Join(",", batch.Select((_, i) => $"(@GroupId{i}, @GoodId{i}, 0, 0, @CountFact{i}, @Price{i})"));

                var p = new DynamicParameters();
                for (int i = 0; i < batch.Length; i++)
                {
                    var goodUuidPrice = goodsUuidPrice[batch[i].Uuid];
                    p.Add($"GroupId{i}", groupId);
                    p.Add($"GoodId{i}", goodUuidPrice.id);
                    p.Add($"CountFact{i}", batch[i].CountFact);
                    p.Add($"Price{i}", goodUuidPrice.price);
                }
                db.Execute(sql, p);
            }
        }

        var summaryGroups = body.Groups.SelectMany(x => x.Goods).GroupBy(x => x.Uuid)
            .Select(x => new { uuid = x.Key, countFact = x.Sum(x=>x.CountFact) });
        const int batchSize1 = 100;
        foreach (var batch in summaryGroups.Select((item, index) => new { item, index })
                                   .GroupBy(x => x.index / batchSize1)
                                   .Select(g => g.Select(x => x.item).ToArray()))
        {
            var sql = "INSERT INTO stocktakingsummarygoods (StocktakingId, GoodId, CountDb, CountFact, Price) VALUES " +
                      string.Join(",", batch.Select((_, i) => $"(@StocktackingId{i}, @GoodId{i},0, @CountFact{i}, @Price{i})"));
            var p = new DynamicParameters();
            for (int i = 0; i < batch.Length; i++)
            {
                var goodUuidPrice = goodsUuidPrice[batch[i].uuid];
                p.Add($"StocktackingId{i}", stocktackingId);
                p.Add($"GoodId{i}", goodUuidPrice.id);
                p.Add($"CountFact{i}", batch[i].countFact);
                p.Add($"Price{i}", goodUuidPrice.price);
            }
            db.Execute(sql, p);
        }

        await tran.CommitAsync();
        return Results.Ok();
    }

    private record GoodPrice(int id, Guid uuid, decimal price);
}
