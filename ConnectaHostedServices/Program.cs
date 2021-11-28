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
            //var configuration = new ConfigurationBuilder()
            //   .SetBasePath(Directory.GetCurrentDirectory())
            //   .AddJsonFile("appsettings.development.json", optional: false, reloadOnChange: true)
            //   .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "production"}.json", optional: true)
            //   .AddEnvironmentVariables()
            //   .Build();

            //Log.Logger = new LoggerConfiguration()
            //    .ReadFrom.Configuration(configuration)
            //    .Enrich.FromLogContext()
            //    .WriteTo.Console()
            //    .CreateLogger();

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


        //static void Main(string[] args)
        //{
        //    CreateHostBuilder().Build().Run();
        //    //var configuration = new ConfigurationBuilder()
        //    //   .SetBasePath(Directory.GetCurrentDirectory())
        //    //   .AddJsonFile("appsettings.development.json", optional: false, reloadOnChange: true)
        //    //   .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "production"}.json", optional: true)
        //    //   .AddEnvironmentVariables()
        //    //   .Build();

        //    //Log.Logger = new LoggerConfiguration()
        //    //    .ReadFrom.Configuration(configuration)
        //    //    .Enrich.FromLogContext()
        //    //    .WriteTo.Console()
        //    //    .CreateLogger();

        //    //var host = Host.CreateDefaultBuilder()

        //    //    //.ConfigureServices((context, services) =>
        //    //    //{
        //    //    //    services.AddSingleton<BaseService, PlantService>();
        //    //    //    services.AddSingleton<BaseService, PaymentCondition>();
        //    //    //    services.AddSingleton<BaseService, Vendor>();
        //    //    //    ServiceLocator.SetLocatorProvider(services.BuildServiceProvider());
        //    //    //})

        //    //    .ConfigureWebHostDefaults(webBuilder =>
        //    //    {
        //    //        webBuilder.UseStartup<Startup>();
        //    //    })
        //    //    .UseSerilog()
        //    //    .Build();

        //    //var host = CreateHostBuilder(args).Build();

        //    //await Task.WhenAny(
        //    //    host.RunAsync()
        //    //);

        //    //Log.Logger.Information("Starting application");
        //    //ServiceLocator.SetLocatorProvider(host.Services.BuildServiceProvider());

        //    //var svc = ActivatorUtilities.CreateInstance<PlantService>(host.Services, configuration);
        //    //svc.ExecuteAsync(new System.Threading.CancellationToken());
        //}

        public static IWebHostBuilder CreateHostBuilder()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddEnvironmentVariables()
                .AddJsonFile($"appsettings.development.json", optional: true, reloadOnChange: true)
                //.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true)
                .Build();

            var host = new WebHostBuilder()
                .UseConfiguration(config)
                .UseStartup<Startup>()
                .ConfigureLogging(logging =>
                {
                    logging.AddConfiguration(config.GetSection("Logging"));
                    logging.AddConsole();
                }
                )
                .UseSerilog()
                .UseUrls("http://*:5001;http://localhost:5001;https://localhost:5001");


            return host;
        }


        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //    Host.CreateDefaultBuilder(args)
        //        .ConfigureWebHostDefaults(webBuilder =>
        //        {
        //            webBuilder.UseStartup<Startup>();
        //        })
        //    .UseSerilog();

        //public static IHostBuilder CreateHostBuilder()
        //{
        //    var configuration = new ConfigurationBuilder()
        //       .SetBasePath(Directory.GetCurrentDirectory())
        //       .AddJsonFile("appsettings.development.json", optional: false, reloadOnChange: true)
        //       .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "production"}.json", optional: true)
        //       .AddEnvironmentVariables()
        //       .Build();

        //    Log.Logger = new LoggerConfiguration()
        //        .ReadFrom.Configuration(configuration)
        //        .Enrich.FromLogContext()
        //        .WriteTo.Console()
        //        .CreateLogger();

        //    var host = new IHost
        //        .ConfigureServices((context, services) =>
        //        {

        //            services.AddTransient<BaseService, PlantService>();
        //            services.AddTransient<BaseService, PaymentCondition>();
        //            services.AddTransient<BaseService, Vendor>();


        //        })
        //        .UseSerilog();

        //    return host;

        //    //var paymentSvc = ActivatorUtilities.CreateInstance<PlantService>(host.Services, configuration);

        //}

        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //Host.CreateDefaultBuilder(args)
        //    .UseStartup<Startup>();



    }
}