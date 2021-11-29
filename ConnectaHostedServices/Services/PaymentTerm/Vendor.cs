﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ConnectaHostedServices.Services.PaymentTerm
{
    public class Vendor : BaseService
    {
        private readonly ILogger<Vendor> _logger;
        private readonly IConfiguration _configuration;

        public Vendor(IServiceScopeFactory serviceScopeFactory, IConfiguration configuration, ILogger<Vendor> logger) :
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
