using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using OnlineCashRmk.Services;
using System.IO;
using Serilog;
using Serilog.Configuration;

namespace OnlineCashRmk
{
    class ConfigureServices
    {
        public static void ConfigureService(ServiceCollection services)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(AppContext.BaseDirectory))
            .AddJsonFile("appsettings.json", optional: true);
            services.AddDbContext<DataContext>()
                .AddSingleton<BarCodeScanner>()
                .AddSingleton<ICashRegisterService, AtolService>()
                .AddLogging(configure => { configure.AddSerilog(); configure.SetMinimumLevel(LogLevel.Error | LogLevel.Warning); })
                .AddScoped<ISynchService, SynchService>()
                .AddSingleton<ISynchBackgroundService, SynchBackgroundService>()
                .AddSingleton<SynchBuyersService>()
                .AddTransient<IConfiguration>(_ => builder.Build())
                .AddScoped<Form1>()
                .AddScoped<FormMenu>()
                .AddTransient<PayForm>()
                .AddTransient<FormWriteOf>()
                .AddTransient<FormArrival>()
                .AddTransient<FormStocktaking>()
                .AddTransient<FormCashMoney>()
                .AddTransient<FormNewGood>()
                .AddTransient<FormFindGood>();
            
        }
    }
}
