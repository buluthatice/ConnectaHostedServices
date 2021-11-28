using ConnectaHostedServices.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NCrontab;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConnectaHostedServices.Scheduler
{
    public abstract class ScheduledProcessor : ScopedProcessor
    {
        private DateTime _nextRun;
        private CrontabSchedule _schedule;
        protected ScheduledProcessor(IServiceScopeFactory serviceScopeFactory, BaseService service) : base(serviceScopeFactory)
        {            
            string name = this.GetType().Name;
            //string schedule = CronJobConfig?.CronJobExpression;
            try
            {
                _schedule = CrontabSchedule.Parse("");
                //_nextRun = _schedule.GetNextOccurrence(DateTime.Now);
            }
            catch (Exception)
            {
                _schedule = null;
            }
        }

        public override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            string taskName = this.GetType().Name.ToString();
            try
            {
                do
                {
                    //bool IsActive = GetConfigurationStatus(taskName);
                    if (_schedule == null)
                        return;
                    var now = DateTime.Now;
                    //now > _nextRun
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
    }
}

