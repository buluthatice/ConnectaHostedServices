using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net;
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
            _logger.LogInformation("Vendor servis calling...{time}", DateTime.Now.ToString());

            try
            {
                if (ApiConnect("GET"))
                    _logger.LogInformation("Vendor servis worked successfully...{time}", DateTime.Now.ToString());
                else
                    _logger.LogInformation("Vendor servis worked failure...{time}", DateTime.Now.ToString());
            }
            catch (WebException WebEx)
            {
                _logger.LogError("Vendor servis worked failure...{time}", DateTime.Now.ToString());
                using (var stream = WebEx.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    _logger.LogError(" Error detail: {detail}, Message: {message} ", reader.ReadToEnd(), WebEx.Message);
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("Vendor servis worked failure...{time}", DateTime.Now.ToString());

                _logger.LogError(" Error detail: {detail}, Message: {message} ", ex.StackTrace, ex.Message);

            }
            await Task.CompletedTask;
        }

    }
}
