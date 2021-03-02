using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;

/// <summary>
/// °Ñ¦Ò
/// </summary>
/// <see cref="https://www.dotblogs.com.tw/supershowwei/2020/12/14/165426"/>
namespace WorkerService1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .CreateLogger();

            try
            {
                Log.Information("Worker Service start.");
                CreateHostBuilder(args).Build().Run();
                Log.Information("Worker Service end.");
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Worker Service terminated unexpectedly.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                });

        /// <summary>
        /// used by Serilog to read appsettings.json
        /// </summary>
        public static IConfiguration Configuration => new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
            .AddJsonFile($"appsettings.{Environment.MachineName}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();
    }
}
