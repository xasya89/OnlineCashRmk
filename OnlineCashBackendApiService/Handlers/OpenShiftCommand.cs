using Dapper;
using Microsoft.AspNetCore.Mvc;
using OnlineCashBackendApiService.Services;
using OnlineCashTransportModels;

namespace OnlineCashBackendApiService.Handlers;

public static class OpenShiftCommand
{
    public static async Task<IResult> Handler([FromBody] OpenShiftTransportModel body, IDbContextFactory factory)
    {
        using var db = factory.CreateDbContext();
        var openedShift = await db.QuerySingleAsync<int>("SELECT COUNT(*) FROM shifts WHERE Stop IS NULL");
        if (openedShift > 0)
            return Results.BadRequest<string>("Смена уже открыта");
        await db.ExecuteAsync(@"INSERT INTO shifts 
                    (ShopId, CashierId, Start, Stop, SumAll, SumSell, SumIncome, SumOutcome, SumCreditDelivery, SumCreditRepayment, SumElectron, SumNoElectron, Uuid, SumReturnCash, SumReturnElectron)
                    VALUES (1, 1, @Start, null, 0, 0, 0, 0, 0, 0, 0, 0, @Uuid, 0, 0)",
            new { Start=body.start, Uuid = body.uuid });
        return Results.Ok();
    }
}
