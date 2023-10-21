using Microsoft.Extensions.DependencyInjection;
using NCrontab;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Infrastructure.Services
{
    public abstract class CronScheduledService : ScopedService
    {
        #region Properties

        private CrontabSchedule CrontabSchedule { get; }

        private DateTime NextRun { get; set; }

        protected abstract string Schedule { get; }

        #endregion

        #region Constructor

        protected CronScheduledService(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {
            CrontabSchedule = CrontabSchedule.Parse(Schedule);
            NextRun = CrontabSchedule.GetNextOccurrence(DateTime.Now); 
        }

        #endregion

        #region Methods

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            do
            {
                var now = DateTime.Now;

                if (now > NextRun)
                {
                    await Process();
                    NextRun = CrontabSchedule.GetNextOccurrence(DateTime.Now);
                }
                await Task.Delay(30000, stoppingToken); //30 seconds delay
            }
            while (!stoppingToken.IsCancellationRequested);
        }

        #endregion
    }
}
