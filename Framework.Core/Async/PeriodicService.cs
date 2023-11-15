using Serilog;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Core.Base
{
    public abstract class PeriodicService<T> : BackgroundService<T> where T : class
    {
        #region Properties     
        
        private int Period { get; }

        #endregion

        #region Constructor

        protected PeriodicService(int period) : base()
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
        protected override Task ExecuteAsync(CancellationToken stoppingToken, Func<Maybe<T>, CancellationToken, Task> func = null) 
        {
            return Task.Factory.StartNew(async () =>
            {
                try
                {
                    bool cancelledTask = false;

                    while (cancelledTask == false)
                    {
                        stoppingToken.ThrowIfCancellationRequested();

                        await Process(stoppingToken, func);

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
                    Log.Fatal("PeriodicService : " + ex.Message);
                }
            }, stoppingToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        protected abstract Task Process(CancellationToken stoppingToken, Func<Maybe<T>, CancellationToken, Task> func = null);

        #endregion
    }
}