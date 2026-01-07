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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Hosting;

namespace OnlineCashRmk
{
    class ConfigureServices
    {
        public static void ConfigureService(ServiceCollection services)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(AppContext.BaseDirectory))
            .AddJsonFile("appsettings.json", optional: true);
            IConfiguration configuration = builder.Build();
            services
                .AddHttpClient()
                //.AddDbContextFactory<DataContext>(opt=>opt.UseSqlite("Data Source=CustomerDB.db;"))
                .AddDbContextFactory<DataContext>(opt => opt.UseSqlite("Data Source=shop-database.db"))
                .AddSingleton<BarCodeScanner>()
                .AddLogging(configure => { configure.AddSerilog(); configure.SetMinimumLevel(LogLevel.Error | LogLevel.Warning); })
                .AddScoped<ISynchService, SynchService>()
                //.AddSingleton<ISynchBackgroundService, SynchBackgroundService>()
                .AddTransient<IConfiguration>(_ => configuration)
                .AddScoped<Form1>()
                .AddScoped<FormMenu>()
                //.AddTransient<PayForm>()
                .AddTransient<FormPaymentCombine>()
                .AddTransient<FormWriteOf>()
                .AddTransient<FormArrival>()
                .AddTransient<FormStocktaking>()
                .AddTransient<FormCashMoney>()
                .AddTransient<FormNewGood>()
                .AddTransient<FormFindGood>()
                .AddTransient<FormHistory>()
                ;
            
        }
    }
}
