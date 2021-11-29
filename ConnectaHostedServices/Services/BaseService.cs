using ConnectaHostedServices.Scheduler;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NCrontab;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConnectaHostedServices.Services
{
    public abstract class BaseService : ScopedProcessor
    {
        private readonly bool _isActive;
        private readonly string _uri;

        private DateTime _nextRun;
        private readonly CrontabSchedule _schedule;

        private readonly ILogger<BaseService> _logger;
        private readonly IConfiguration _configuration;

        public BaseService(IServiceScopeFactory serviceScopeFactory, ILogger<BaseService> logger, IConfiguration configuration) :
            base(serviceScopeFactory)
        {
            this._logger = logger;
            this._configuration = configuration;
            string name = this.GetType().Name;

            dynamic conf = GetServiceConfiguration(name);

            _isActive = bool.Parse(conf.IsActive);
            _uri = conf.Uri;
            try
            {
                _schedule = CrontabSchedule.Parse(conf.Schedule);
                _nextRun = _schedule.GetNextOccurrence(DateTime.Now);
            }
            catch (Exception)
            {
                _schedule = null;
            }
        }

        public override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                do
                {
                    if (_schedule == null)
                        return;
                    var now = DateTime.Now;
                    if (now > _nextRun)
                    {
                        await Process();
                        _nextRun = _schedule.GetNextOccurrence(DateTime.Now);
                    }
                    await Task.Delay(5000, stoppingToken); //5 seconds delay
                }
                while (!stoppingToken.IsCancellationRequested);
            }
            catch (Exception)
            {


            }
        }

        public dynamic GetServiceConfiguration(string name)
        {

            dynamic obj = new ExpandoObject();
            obj.IsActive = _configuration[$"Services:{name}:IsActive"];
            obj.Uri = _configuration[$"Services:{name}:Uri"];
            obj.Schedule = _configuration[$"Services:{name}:Schedule"];
            return obj;
        }
    }
}
