using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Serilog.Events;
using Microsoft.Extensions.Hosting;
using OnlineCashRmk.Services;
using Microsoft.EntityFrameworkCore.Internal;

namespace OnlineCashRmk
{
    static class Program
    {
        public static string HttpClientName = "api";
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static async Task Main()
        {
            if (InstanceCheck())
            {
                var config = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory) // важно для WinForms!
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();
                Application.SetHighDpiMode(HighDpiMode.SystemAware);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                //Application.Run(new Form1());
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
                    .WriteTo.File(Path.Combine("logs", "log.log"), rollingInterval: RollingInterval.Day).CreateLogger();

                var host = Host.CreateDefaultBuilder()
             /*
              * .ConfigureAppConfiguration((context, builder) =>
             {
                 // Add other configuration files...
                 builder.AddJsonFile("appsettings.json", optional: true);
             })
             */
             .ConfigureServices((context, services) =>
             {
                 services.AddSingleton<IConfiguration>(config);
                 services.AddHttpClient(HttpClientName, (sp, client) =>
                 {
                     var _config = sp.GetRequiredService<IConfiguration>();
                     client.BaseAddress = new Uri(config.GetConnectionString("ServerApi"));
                     client.DefaultRequestHeaders.Add("X-shopDbName", "shop7");
                 });
                 services.AddHttpClient()
                //.AddDbContextFactory<DataContext>(opt=>opt.UseSqlite("Data Source=CustomerDB.db;"))
                .AddDbContextFactory<DataContext>(opt => opt.UseSqlite("Data Source=app.db;"))
                .AddLogging(configure => { configure.AddSerilog(); configure.SetMinimumLevel(LogLevel.Error | LogLevel.Warning); })
                
                .AddSingleton<BarCodeScanner>()
                .AddScoped<ISynchService, SynchService>()
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
                .AddSingleton<ICashRegisterService, CashRgisterService>()
                .AddHostedService<SynchBackgroundService>()
                .AddHostedService<ApplicationBackgroundService>();
             })
             .ConfigureLogging(logging =>
             {
                 // Add other loggers...
             })
             .UseSerilog()
             .UseConsoleLifetime()
             .Build();

                using (var scope = host.Services.CreateScope())
                using (var db = scope.ServiceProvider.GetRequiredService<DataContext>())
                    db.Database.Migrate();

                CurrentHost = host;

                await host.StartAsync();
            }
        }
        // держим в переменной, чтобы сохранить владение им до конца пробега программы
        static Mutex InstanceCheckMutex;
        static bool InstanceCheck()
        {
            bool isNew;
            InstanceCheckMutex = new Mutex(true, "OnlineCashRmk", out isNew);
            return isNew;
        }

        private static IHost CurrentHost;
        public static async Task StopedHost()
        {
            Application.Exit();
            await CurrentHost.StopAsync();
        }
    }
}
