using Dapper;
using Microsoft.AspNetCore.Mvc;
using OnlineCashBackendApiService.Handlers;
using OnlineCashBackendApiService.Services;
using OnlineCashTransportModels;
using System;
using System.Diagnostics;

namespace OnlineCashBackendApiService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var currentDir = AppDomain.CurrentDomain.BaseDirectory;
            builder.Environment.ContentRootPath = currentDir;
            var configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
            if (!File.Exists(configPath))
            {
                // Запишем в журнал событий Windows или файл
                System.Diagnostics.Trace.WriteLine("ERROR: appsettings.json not found!");
            }

            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                var logFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "crash.log");
                var ex = e.ExceptionObject as Exception;
                var message = $"[{DateTime.Now}] Unhandled exception:\n{ex?.ToString()}\n";
                File.AppendAllText(logFile, message);
            };

            // Также для async-исключений (если используете)
            TaskScheduler.UnobservedTaskException += (sender, e) =>
            {
                var logFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "unobserved-task-exception.log");
                File.AppendAllText(logFile, $"[{DateTime.Now}] {e.Exception}\n");
                e.SetObserved(); // предотвращает падение
            };

            // Add services to the container.
            bool isService = !(Debugger.IsAttached || args.Contains("--console"));
            if (isService && OperatingSystem.IsWindows())
                builder.Host.UseWindowsService();
            builder.Services.AddAuthorization();
            builder.Services.AddDistributedMemoryCache();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddTransient<IDbContextFactory, DbContextFactory>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseMiddleware<IdempotencyMiddleware>();

            var group = app.MapGroup("/online-cash/v3");


            group.MapPost("/open-shift", OpenShiftCommand.Handler).WithName("open-shift").WithOpenApi();
            group.MapPost("/close-shift", CloseShiftCommand.Handler).WithName("close-shift").WithOpenApi();
            group.MapPost("/create-check", CreateCheck.Handler).WithName("create-check").WithOpenApi();
            group.MapGet("/manuals/goods", ManualGoodsQuery.Handler);
            group.MapGet("/manuals/suppliers", ManualSupplierQuery.Query);
            group.MapPost("/create-arrival", CreateArrivalCommand.Handler).WithName("create-arrival").WithOpenApi();
            group.MapPost("/create-revaluation", CreateRevaluationCommand.Handler).WithName("create-revaluation").WithOpenApi();
            group.MapPost("/create-writeof", CreateWriteOfCommand.Handler).WithName("create-writeof").WithOpenApi();
            group.MapPost("/create-stocktacking", CreateStockTackingCommand.Handler).WithName("create-stocktacking").WithOpenApi();
            group.MapPost("/create-cashmoney", CreateCashMoneyCommand.Handler).WithName("create-cashmoney").WithOpenApi();

            app.Run();
        }
    }
}


public record OpenShiftBody(Guid uuid);
public record CloseShiftBody();