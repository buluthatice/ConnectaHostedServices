using ConnectaHostedServices.Services;
using ConnectaHostedServices.Services.PaymentTerm;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;
using System;
using System.Dynamic;
using System.IO;
using System.Threading.Tasks;

namespace ApplicationTemplate
{
    public class Program
    {

        public static async Task Main(string[] args)
        {
            var host = new HostBuilder()
                .ConfigureHostConfiguration(configHost =>
                {
                    configHost.Sources.Clear();
                    configHost.SetBasePath(Directory.GetCurrentDirectory());
                    configHost.AddJsonFile("Config/hostsettings.json", optional: true);
                    configHost.AddEnvironmentVariables(prefix: "APP_");
                    configHost.AddCommandLine(args);
                })
                .ConfigureAppConfiguration((hostContext, configApp) =>
                {
                    configApp.AddJsonFile("appsettings.development.json", optional: false, reloadOnChange: true);
                    configApp.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "production"}.json", optional: true);
                    configApp.AddEnvironmentVariables(prefix: "APP_");
                    configApp.AddCommandLine(args);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<IHostedService, PlantService>();
                    services.AddSingleton<IHostedService, PaymentCondition>();
                    services.AddSingleton<IHostedService, Vendor>();
                })
                .ConfigureLogging((hostContext, configLogging) =>
                {
                    configLogging.AddConsole();
                    configLogging.AddDebug();
                })
                .UseSerilog()
                .UseConsoleLifetime()
                .Build();

            await host.RunAsync();
        }       
    }
}