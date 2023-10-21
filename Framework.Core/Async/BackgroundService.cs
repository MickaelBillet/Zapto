using System;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Core.Base
{
    public abstract class BackgroundService<T> : IDisposable where T : class
    {
        bool disposed = false;

        #region Properties 

        private Task ExecutingTask { get; set; }
        private CancellationTokenSource StoppingCts { get; set; }

        #endregion

        #region Methods

        protected abstract Task ExecuteAsync(CancellationToken stoppingToken, Func<Maybe<T>, CancellationToken, Task> func = null);

        public virtual Task StartAsync(Func<Maybe<T>, CancellationToken, Task> func, CancellationToken cancellationToken)
        {            
            if ((this.ExecutingTask == null) || (this.ExecutingTask.IsCompleted))
            {
                this.StoppingCts = new CancellationTokenSource();

                this.ExecutingTask = ExecuteAsync(this.StoppingCts.Token, func);
            }

            return this.ExecutingTask;
        }

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            // Stop called without start
            if (this.ExecutingTask == null)
            {
                return;
            }

            try
            {
                // Signal cancellation to the executing method
                this.StoppingCts.Cancel();
            }
            finally
            {
                // Wait until the task completes or the stop token triggers
                await Task.WhenAny(this.ExecutingTask, Task.Delay(Timeout.Infinite, cancellationToken));
            }
        }

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                this.StoppingCts.Cancel();
            }

            // Free any unmanaged objects here.
            //
            disposed = true;
        }

        #endregion
    }
}
