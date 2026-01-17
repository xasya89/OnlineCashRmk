using Dapper;
using Microsoft.AspNetCore.Mvc;
using OnlineCashBackendApiService.Services;
using OnlineCashTransportModels;
using System;

namespace OnlineCashBackendApiService.Handlers;

public static class CreateCashMoneyCommand
{
    public static async Task Handler([FromBody] CashMoneyTransportModel body, IDbContextFactory db)
    {
        using var con = db.CreateDbContext();
        await con.ExecuteAsync(@"INSERT INTO cashmoneys (ShopId, Uuid, `Create`, TypeOperation, Sum, Note)
            VALUES (1, @Uuid, @Create, @TypeOperation, @Sum, @Note)",
            new
            {
                Uuid=body.Uuid,
                Create=body.Create,
                TypeOperation=body.TypeOperation,
                Sum=body.Sum,
                Note=body.Note,
            });
    }
}
