using Dapper;
using K4os.Hash.xxHash;
using Microsoft.AspNetCore.Mvc;
using OnlineCashBackendApiService.Services;
using OnlineCashTransportModels;
using OnlineCashTransportModels.Shared;

namespace OnlineCashBackendApiService.Handlers;

public static class CloseShiftCommand
{
    private record CheckSell(int id, TypeSell TypeSell, decimal SumAll, decimal SumCash, decimal SumElectron, decimal SumDiscont);
    private record CheckGood(int GoodId, Guid Uuid, decimal Price, decimal PromotionCount);
    public static async Task<IResult> Handler([FromBody] CloseShiftTransportModel body, IDbContextFactory dbContextFactory)
    {
        using var db = dbContextFactory.CreateDbContext();
        await db.OpenAsync();
        var tran = await db.BeginTransactionAsync();
        var shiftId = await db.QuerySingleAsync<int?>("SELECT MAX(id) FROM shifts WHERE Uuid=@Uuid AND Stop IS NULL",
            new { Uuid=body.uuid });
        if (!shiftId.HasValue)
            return Results.BadRequest<string>("Смена не найдена");
        var checks = await db.QueryAsync<CheckSell>("SELECT id, TypeSell, SumAll, SumCash, SumElectron, SumDiscont FROM checksells WHERE ShiftId=" + shiftId);
        decimal sumAll = checks.Where(x => x.TypeSell == TypeSell.Sell).Sum(x => x.SumAll) - checks.Where(x => x.TypeSell == TypeSell.Return).Sum(x => x.SumAll);
        decimal sumSell = checks.Where(x => x.TypeSell == TypeSell.Sell).Sum(x => x.SumAll);
        decimal sumCash = checks.Where(x => x.TypeSell == TypeSell.Sell).Sum(x => x.SumCash);
        decimal sumElectron = checks.Where(x => x.TypeSell == TypeSell.Sell).Sum(x => x.SumElectron);
        decimal sumReturnCash = checks.Where(x => x.TypeSell == TypeSell.Return).Sum(x => x.SumCash);
        decimal sumReturnElectron = checks.Where(x => x.TypeSell == TypeSell.Return).Sum(x => x.SumElectron);
        decimal sumDiscount = checks.Sum(x => x.SumDiscont);
        var ids = checks.Select(x => x.id);
        var checkGoods = await db.QueryAsync<CheckGood>(@"SELECT cg.GoodId, g.Uuid, cg.Price, cg.PromotionCount 
            FROM checkgoods cg INNER JOIN goods g ON cg.goodId=g.id 
            WHERE CheckSellId IN @ids",
            new { Ids = ids });
        decimal sumPromotion = checkGoods.Sum(x=>x.Price * x.PromotionCount);
        await db.ExecuteAsync(@"UPDATE shifts 
            SET Stop=@Stop, SumAll=@SumAll, SumSell=@SumSell, SumElectron=@SumElectron, SumNoElectron=@SumNoElectron,
            SumReturnCash=@SumReturnCash, SumReturnElectron=@SumReturnElectron, SumPromotion=@SumPromotion, SumDiscount=@SumDiscount 
            WHERE id=@Id",
            new {
                Id=shiftId.Value,
                Stop=body.stop,
                SumAll= sumAll,
                SumSell= sumSell,
                SumNoElectron = sumCash, 
                SumElectron= sumElectron,
                SumReturnCash = sumReturnCash,
                SumReturnElectron = sumReturnElectron,
                SumPromotion= sumPromotion,
                SumDiscount = sumDiscount,
            });
        //Авто списание
        if (sumPromotion > 0)
        {
            var bodyWriteOf = new CreateWriteOfTransportModel
            {
                DateCreate = body.stop,
                Uuid = Guid.NewGuid(),
                Note = "Списание по акции 2+1",
                Positions = checkGoods.Where(x => x.PromotionCount > 0)
                .GroupBy(x => x.GoodId)
                .Select(x => new CreateWriteOfPositionTransportModel
                {
                    Uuid = x.First().Uuid,
                    Price = x.First().Price,
                    Count = x.Sum(x => x.PromotionCount),
                })
            };
            await CreateWriteOfCommand.Handler(bodyWriteOf, dbContextFactory);
        }
        await tran.CommitAsync();
        return Results.Ok();
    }
}
