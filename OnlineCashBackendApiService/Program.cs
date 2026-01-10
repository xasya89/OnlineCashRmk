using Dapper;
using Microsoft.AspNetCore.Mvc;
using OnlineCashBackendApiService.Handlers;
using OnlineCashBackendApiService.Services;
using OnlineCashTransportModels;
using System;

namespace OnlineCashBackendApiService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

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

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapPost("/open-shift", OpenShiftCommand.Handler).WithName("open-shift").WithOpenApi();
            app.MapPost("/close-shift", CloseShiftCommand.Handler).WithName("close-shift").WithOpenApi();
            app.MapPost("/create-check", CreateCheck.Handler).WithName("create-check").WithOpenApi();
            app.MapGet("/manuals/goods", ManualGoodsQuery.Handler);
            app.MapGet("/manuals/suppliers", ManualSupplierQuery.Query);
            app.MapPost("/create-arrival", CreateArrivalCommand.Handler).WithName("create-arrival").WithOpenApi();
            app.MapPost("/create-writeof", CreateWriteOfCommand.Handler).WithName("create-writeof").WithOpenApi();
            app.MapPost("/create-stocktacking", CreateWriteOfCommand.Handler).WithName("create-stocktacking").WithOpenApi();

            app.Run();
        }
    }
}


public record OpenShiftBody(Guid uuid);
public record CloseShiftBody();