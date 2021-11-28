using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ConnectaHostedServices.Services.PaymentTerm
{
    public class PlantService : BaseService
    {
        private readonly ILogger<PlantService> _logger;
        private readonly IConfiguration _configuration;

        public PlantService(IServiceScopeFactory serviceScopeFactory, IConfiguration configuration, ILogger<PlantService> logger) : 
            base(serviceScopeFactory, logger, configuration)
        {
            this._logger = logger;
        }

        public override async Task ProcessInScope(IServiceProvider serviceProvider)
        {

            await Task.CompletedTask;
        }

    }
}
