using Dapper;
using K4os.Hash.xxHash;
using Microsoft.AspNetCore.Mvc;
using OnlineCashBackendApiService.Services;
using OnlineCashTransportModels;

namespace OnlineCashBackendApiService.Handlers;

public static class CloseShiftCommand
{
    private record CheckSell(int TypeSell, decimal SumAll, decimal SumCash, decimal SumElectron);
    public static async Task<IResult> Handler([FromBody] CloseShiftTransportModel body, IDbContextFactory dbContextFactory)
    {
        using var db = dbContextFactory.CreateDbContext();
        var shiftId = await db.QuerySingleAsync<int?>("SELECT MAX(id) FROM shifts WHERE stop IS NULL");
        if (!shiftId.HasValue)
            return Results.BadRequest<string>("Смена не открыта");
        var checks = await db.QueryAsync<CheckSell>("SELECT TypeSell, SumAll, SumCash, SumElectron FROM checksells WHERE ShiftId="+shiftId);
        decimal sumAll = checks.Where(x => x.TypeSell == 0).Sum(x => x.SumAll) - checks.Where(x => x.TypeSell == 1).Sum(x => x.SumAll);
        decimal sumCash = checks.Where(x => x.TypeSell == 0).Sum(x => x.SumCash);
        decimal sumElectron = checks.Where(x => x.TypeSell == 0).Sum(x => x.SumElectron);
        decimal sumReturnCash = checks.Where(x => x.TypeSell == 1).Sum(x => x.SumCash);
        decimal sumReturnElectron = checks.Where(x => x.TypeSell == 1).Sum(x => x.SumElectron);
        await db.ExecuteAsync(@"UPDATE shifts 
            SET Stop=@Stop, SumAll=@SumAll, SumSell=@SumAll, SumElectron=@SumElectron, SumNoElectron=@SumNoElectron,
            SumReturnCash=@SumReturnCash, SumReturnElectron=@SumReturnElectron WHERE id=@Id",
            new {
                Id=shiftId.Value,
                Stop=body.stop,
                SumAll= sumAll,
                SumNoElectron = sumCash, 
                SumElectron= sumElectron,
                SumReturnCash = sumReturnCash,
                SumReturnElectron = sumReturnElectron
            });
        return Results.Ok();
    }
}
