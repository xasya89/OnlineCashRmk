using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using System.Threading;
using Microsoft.Extensions.Logging;

namespace OnlineCashRmk.Services
{
    public class SynchRabbitBackgroundService: BackgroundService
    {
        IServiceScopeFactory _scopeFactory;
        ILogger<SynchRabbitBackgroundService> _logger;
        IConfiguration _configuration;
        public SynchRabbitBackgroundService(IServiceScopeFactory scopeFactory,
            IConfiguration configuration,
            ILogger<SynchRabbitBackgroundService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var scope = _scopeFactory.CreateScope();
            var dbFactory = scope.ServiceProvider.GetService<IDbContextFactory<DataContext>>();
            using var db = dbFactory.CreateDbContext();
        }
    }
}
