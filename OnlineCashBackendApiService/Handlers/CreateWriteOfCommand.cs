using Dapper;
using Microsoft.AspNetCore.Mvc;
using OnlineCashBackendApiService.Services;
using OnlineCashTransportModels;
using System;

namespace OnlineCashBackendApiService.Handlers;

public static class CreateWriteOfCommand
{
    public static async Task<IResult> Handler([FromBody] CreateWriteOfTransportModel body, IDbContextFactory dbContextFactory)
    {
        using var db= dbContextFactory.CreateDbContext();
        await db.OpenAsync();
        using var transaction = await db.BeginTransactionAsync();

        var uuids = body.Positions.Select(x => x.Uuid);
        var goods = await db.QueryAsync<Good>("SELECT id, uuid FROM goods WHERE Uuid IN @Uuids", new { Uuids = uuids });
        var writeofId = await db.QuerySingleAsync<int>(@"INSERT INTO writeofs 
            (DateWriteof, ShopId, Note, SumAll, IsSuccess,Status, Uuid)
            VALUES (@DateWriteof, 1, @Note, @SumAll, 1,2, @Uuid);
            SELECT LAST_INSERT_ID()", new
        {
            DateWriteof=body.DateCreate,
            Note=body.Note,
            SumAll=body.Positions.Sum(x=>x.Price * x.Count),
            Uuid=body.Uuid,
        });

        foreach (var item in body.Positions)
            await db.ExecuteAsync(@"INSERT INTO writeofgoods (WriteofId, GoodId, Count, Price) 
                VALUES (@WriteofId, @GoodId, @Count, @Price)",
                new { 
                    WriteofId=writeofId, 
                    GoodId=goods.Where(x=>x.uuid==item.Uuid).First().id, 
                    Price=item.Price, 
                    Count=item.Count 
                });

        await transaction.CommitAsync();
        return Results.Ok();
    }

    private record Good(int id, Guid uuid);
}
