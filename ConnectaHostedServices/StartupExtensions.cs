using ConnectaHostedServices.Services.PaymentTerm;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ApplicationTemplate
{
    public static class StartupExtensions
    {
        public static void AddScheduledJobs(this IServiceCollection services)
        {
            services.AddSingleton<IHostedService, PaymentCondition>();
            services.AddSingleton<IHostedService, PlantService>();
            services.AddSingleton<IHostedService, Vendor>();
        }
    }




}