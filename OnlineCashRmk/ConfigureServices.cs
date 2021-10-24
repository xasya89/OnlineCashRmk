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
                .AddLogging(configure => configure.AddConsole())
                .AddScoped<ISynchService, SynchService>()
                .AddSingleton<ISynchBackgroundService, SynchBackgroundService>()
                .AddTransient<IConfiguration>(_=>builder.Build())
                .AddScoped<Form1>();
        }
    }
}
