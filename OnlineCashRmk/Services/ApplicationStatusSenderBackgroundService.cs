using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OnlineCashTransportModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace OnlineCashRmk.Services;

internal class ApplicationStatusSenderBackgroundService(
    ILogger<ApplicationStatusSenderBackgroundService> logger,
    IHttpClientFactory httpClientFactory, 
    IDbContextFactory<DataContext> dbContextFactory
    ) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Send();
        PeriodicTimer periodicTimer = new PeriodicTimer(TimeSpan.FromMinutes(5));
        while (await periodicTimer.WaitForNextTickAsync() & !stoppingToken.IsCancellationRequested)
            await Send();
    }

    private async Task Send()
    {
        try
        {
            using var db = dbContextFactory.CreateDbContext();
            var httpClient = httpClientFactory.CreateClient(Program.HttpClientName);
            var lastSynch = await db.DocSynches.Where(x => !x.SynchStatus)
                .OrderBy(x => x.Create)
                .FirstOrDefaultAsync();
            var version_app = Version.TryParse(FileVersionInfo.GetVersionInfo(Application.ExecutablePath).FileVersion, out var v) ? v : new Version(0, 0, 0);
            var body = new ApplicationStatusTransportModel
            {
                LastDocSynch = lastSynch?.Create,
                LastSynchTypeDoc = lastSynch?.TypeDoc,
                Version = version_app.ToString()
            };
            var message = new HttpRequestMessage(HttpMethod.Post, "application-status")
            {
                Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json")
            };
            message.Headers.Add("X-Document-UUID", Guid.NewGuid().ToString());
            var response = await httpClient.SendAsync(message);
        }
        catch (SystemException ex)
        {
            logger.LogError("Ошибка отправки статистики на сервер");
        }
        catch (Exception ex)
        {
            logger.LogError("Ошибка отправки статистики на сервер");
        }
    }
}
