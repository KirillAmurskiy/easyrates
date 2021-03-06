using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace EasyRates.WriterApp.AspNet
{
    public class Program
    {
        public const string AppName = "EasyRates.Writer";

        public static void Main(string[] args)
        {
            Console.Title = AppName;

            IHostBuilder host = CreateHostBuilder(args);

            try
            {
                Log.Logger.Information("Starting web host");
                host.Build().Run();
            }
            catch (Exception ex)
            {
                Log.Logger.Fatal(ex, "Web Host terminated unexpectedly");
            }
            finally
            {
                Log.Logger.Information("Finally web host");
                Log.CloseAndFlush();
            }
        }
        
        
        /// <summary>
        /// This method is needed for NSwag client generation during building.
        /// If signature is different it will be error.
        /// </summary>
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            string currentEnv = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ??
                                Environments.Production;

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{currentEnv}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();
            
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.WithProperty("Application", AppName)
                .Enrich.WithProperty("Environment", currentEnv)
                .CreateLogger();
            
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseConfiguration(configuration)
                        .UseSerilog()
                        .UseStartup<Startup>();
                })
                // Add a new service provider configuration
                .UseDefaultServiceProvider((context, options) =>
                {
                    options.ValidateScopes = context.HostingEnvironment.IsDevelopment();
                    options.ValidateOnBuild = true;
                });;
        }
    }
}