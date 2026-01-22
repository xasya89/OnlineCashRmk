using Dapper;
using Microsoft.AspNetCore.Mvc;
using OnlineCashBackendApiService.Services;
using OnlineCashTransportModels;

namespace OnlineCashBackendApiService.Handlers;

public static class CreateArrivalCommand
{
    public static async Task<IResult> Handler([FromBody] CreateArrivalTransportModel body, IDbContextFactory factory)
    {
        using var db = factory.CreateDbContext();
        await db.OpenAsync();
        using var tran = await db.BeginTransactionAsync();

        var uuids = body.Positions.Select(x => x.Uuid);
        var goods = (await db.QueryAsync<Item>("SELECT id, price, uuid FROM Goods WHERE Uuid IN @Uuids",
            new { Uuids = uuids })).ToList();
        /*
        foreach(var item in body.Positions)
        {
            var price = goods.Where(x => x.uuid == item.Uuid).First().price;
            item.PriceSell = price;
        }
        */
        var sumArrival = body.Positions.Sum(x=>x.PriceArrival * x.Count);
        var sumSell = body.Positions.Sum(x=>x.PriceSell * x.Count);
        var arrivalId = await db.QuerySingleAsync<int>(@"INSERT INTO arrivals 
            (Num, DateArrival, SupplierId, ShopId, isSuccess, SumPayments, SumArrival, SumNds, SumSell, Status)
            VALUES (@Num, @DateArrival, @SupplierId, 1, 1, @SumArrival, @SumArrival, @SumArrival, @SumSell, 2);
            SELECT LAST_INSERT_ID();",
            new {
                Num=body.Num,
                DateArrival=body.DateArrival,
                SupplierId=body.SupplierId,
                SumArrival =sumArrival,
                SumSell=sumSell,
            });
        foreach (var item in body.Positions)
        {
            var good = goods.Where(x => x.uuid == item.Uuid).First();
            await db.ExecuteAsync(@"INSERT INTO arrivalgoods (ArrivalId ,GoodId, Count, Price, PriceSell, Nds)
                VALUES (@ArrivalId ,@GoodId, @Count, @Price, @PriceSell, @Nds)",
                new
                {
                    ArrivalId=arrivalId,
                    GoodId=good.id,
                    Count = item.Count,
                    Price = item.PriceArrival,
                    PriceSell=item.PriceSell,
                    Nds=item.Nds,
                });
        }

        await tran.CommitAsync();
        return Results.Ok();
    }

    private record Item(int id, decimal price, Guid uuid);
}
