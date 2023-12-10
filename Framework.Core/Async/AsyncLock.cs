using System;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Core.Base
{
    public class AsyncLock
    {
        #region Property

        private AsyncSemaphore Semaphore { get; set; }

        private Task<Releaser> TaskReleaser { get; set; }

        #endregion

        #region Constructor

        public AsyncLock()
        {
            Semaphore = new AsyncSemaphore(1);
            TaskReleaser = Task.FromResult(new Releaser(this));
        }

        #endregion

        #region Method

        public Task<Releaser> LockAsync()
        {
            var wait = Semaphore.WaitAsync();
            return wait.IsCompleted ?
                TaskReleaser :
                wait.ContinueWith((_, state) => new Releaser((AsyncLock)state),
                    this, CancellationToken.None,
                    TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
        }

        #endregion

        public readonly struct Releaser : IDisposable
        {
            private readonly AsyncLock ToRelease;

            internal Releaser(AsyncLock toRelease) { ToRelease = toRelease; }

            public void Dispose()
            {
                ToRelease?.Semaphore.Release();
            }
        }
    }
}
