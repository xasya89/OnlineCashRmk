using Microsoft.AspNetCore.Mvc;
using OnlineCashBackendApiService.Services;
using OnlineCashTransportModels;
using System.Collections.Generic;

namespace OnlineCashBackendApiService.Handlers.ApplicationStatus;

public static class ReciveApplicationStatus
{
    public static IResult Handler([FromBody] ApplicationStatusTransportModel body, ApplicationStatusService service, IDbContextFactory factory)
    {
        service.Add(factory.TenantName, body);
        return Results.Ok();
    }
}

public static class GetApplicationStates
{
    private record Response(string shopDbName, string version, string? typeDoc, string? lastSynch);
    public static IResult Handler(ApplicationStatusService service, IDbContextFactory factory)
    {
        var result = service.Get().Select(x => new Response(x.shopDbName, x.version, x.typeDoc, x.lastDocSynch?.ToString("dd.MM HH:mm:SS")));
        return Results.Ok<IEnumerable<Response>>(result);
    }
}