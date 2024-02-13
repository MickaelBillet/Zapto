using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Infrastructure.Services
{
    public abstract class ScheduledService : ScopedService
    {
        #region Properties

        protected int Period { get; set; } = 0;

        #endregion

        #region Constructor

        protected ScheduledService(IServiceScopeFactory serviceScopeFactory, int period) : base(serviceScopeFactory)
        {
            this.Period = period;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Launch the process in a task
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Factory.StartNew(async () =>
            { 
                try
                {
                    bool cancelledTask = false;

                    while (cancelledTask == false)
                    {
                        stoppingToken.ThrowIfCancellationRequested();

                        await Process();

                        if (stoppingToken != null)
                        {
                            cancelledTask = stoppingToken.WaitHandle.WaitOne(this.Period);
                        }
                        else
                        {
                            cancelledTask = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Fatal("ScheduledService : " + ex.Message);
                }
            }, stoppingToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        #endregion
    }
}
