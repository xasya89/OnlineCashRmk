using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using OnlineCashRmk.Models;
using OnlineCashRmk.DataModels;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using Flurl.Http;
using System.Text.Json;
using System.Net.Http.Json;
using Microsoft.Extensions.Hosting;
using System.Threading;
using OnlineCashTransportModels;
using Microsoft.Extensions.Logging;

namespace OnlineCashRmk.Services
{
    class SynchBackgroundService(IDocumentSenderService documentSenderService, ILogger<SynchBackgroundService> logger) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            PeriodicTimer periodicTimer = new PeriodicTimer(TimeSpan.FromMinutes(5));
            while (await periodicTimer.WaitForNextTickAsync() & !stoppingToken.IsCancellationRequested)
                try
                {
                    await documentSenderService.SendDocuments();
                }
                catch(SystemException ex)
                {
                    logger.LogError("Ошибка обмена с сервером");
                }
                catch (Exception ex) {
                    logger.LogError("Ошибка обмена с сервером");
                }
        }
    }
}
