using Dapper;
using Microsoft.AspNetCore.Mvc;
using OnlineCashBackendApiService.Services;
using OnlineCashTransportModels;
using OnlineCashTransportModels.Shared;
using Org.BouncyCastle.Utilities.Collections;

namespace OnlineCashBackendApiService.Handlers;

public static class CreateCheck
{
    public static async Task<IResult> Handler([FromBody] CreateCheckTransportModel body, IDbContextFactory factory)
    {
        using var db = factory.CreateDbContext();
        await db.OpenAsync();
        var tran = await db.BeginTransactionAsync();
        var shiftId = await db.QuerySingleAsync<int?>("SELECT MAX(id) FROM shifts WHERE stop IS NULL");
        if (!shiftId.HasValue)
            return Results.BadRequest<string>("Смена не открыта");
        var checkId = await db.QuerySingleAsync<int>(@"INSERT INTO checksells 
            (ShiftId, DateCreate, IsElectron, SumAll, Sum, SumDiscont, TypeSell,SumCash, SumElectron)
            VALUES
            (@ShiftId, @DateCreate, @IsElectron, @SumAll, @SumAll, @SumDiscont, @TypeSell, @SumCash, @SumElectron);
            SELECT LAST_INSERT_ID()",
            new
            {
                ShiftId = shiftId.Value,
                DateCreate=body.DateCreate,
                IsElectron=body.SumElectron>0,
                SumAll= body.SumCash + body.SumElectron,
                SumDiscont=body.SumDiscont,
                TypeSell=body.TypeSell,
                SumCash = body.SumCash,
                SumElectron=body.SumElectron
            });
        await db.ExecuteAsync(@"UPDATE shifts SET 
            SumAll=SumAll + @SumAll, SumSell=SumSell + @SumSell, 
            SumElectron=SumElectron+@SumElectron, SumNoElectron=SumNoElectron + @SumNoElectron, 
            SumReturnCash=SumReturnCash + @SumReturnCash, SumReturnElectron=SumReturnElectron + @SumReturnElectron,
            SumPromotion=SumPromotion + @SumPromotion
            WHERE Id=@Id",
            new
            {
                Id=shiftId,
                SumAll = body.TypeSell == TypeSell.Sell ? body.SumCash + body.SumElectron : -1 * (body.SumCash + body.SumElectron),
                SumSell = body.TypeSell == TypeSell.Sell ? body.SumCash + body.SumElectron : 0,
                SumElectron = body.TypeSell==TypeSell.Sell ? body.SumElectron : 0,
                SumNoElectron = body.TypeSell == TypeSell.Sell ? body.SumCash : 0,
                SumReturnElectron = body.TypeSell == TypeSell.Return ? body.SumElectron : 0,
                SumReturnCash = body.TypeSell == TypeSell.Return ? body.SumCash : 0,
                SumPromotion = body.Positions.Sum(x=>x.PromotionQuantity * x.Price),
            });
        var uuids = body.Positions.Select(x => x.Uuid);
        var goods = await db.QueryAsync<GoodIdUuid>("SELECT id, uuid FROM goods WHERE uuid IN @uuids",
            new { Uuids = uuids });
        foreach (var item in body.Positions)
            await db.ExecuteAsync(@"INSERT INTO checkgoods (CheckSellId, GoodId, Count, Price, PromotionCount) 
                VALUES (@CheckSellId, @GoodId, @Count, @Price, @PromotionCount)", new
            {
                CheckSellId=checkId,
                GoodId= goods.Where(x=>x.uuid==item.Uuid).First().id,
                Count=item.Quantity,
                Price=item.Price,
                PromotionCount = item.PromotionQuantity,
            });
        await tran.CommitAsync();
        return Results.Ok();
    }

    private record GoodIdUuid(int id, Guid uuid);
}
