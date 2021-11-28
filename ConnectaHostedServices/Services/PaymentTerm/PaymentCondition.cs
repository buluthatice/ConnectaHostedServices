using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ConnectaHostedServices.Services.PaymentTerm
{
    public class PaymentCondition : BaseService
    {
        private readonly ILogger<PaymentCondition> _logger;
        private readonly IConfiguration _configuration;

        public PaymentCondition(IServiceScopeFactory serviceScopeFactory, ILogger<PaymentCondition> logger, IConfiguration configuration) : //, bool IsActive, string Uri, string Schedule
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
