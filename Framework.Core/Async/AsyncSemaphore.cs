using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Framework.Core.Base
{
    /// <summary>
    /// Provides a simple wrapper over a SemaphoreSlim to allow synchronization locking inside of async calls. 
    /// </summary>
    public class AsyncSemaphore
    {
        #region Property

        private readonly static Task IsCompleted = Task.FromResult(true);

        private readonly Queue<TaskCompletionSource<bool>> Waiters = new Queue<TaskCompletionSource<bool>>();

        private int CurrentCount { get; set; }

        #endregion

        #region Constructor

        public AsyncSemaphore(int initialCount)
        {
            if (initialCount < 0) throw new ArgumentOutOfRangeException("initialCount");
            CurrentCount = initialCount;
        }

        #endregion

        #region Method

        public Task WaitAsync()
        {
            lock (Waiters)
            {
                if (CurrentCount > 0)
                {
                    --CurrentCount;
                    return IsCompleted;
                }
                else
                {
                    TaskCompletionSource<bool> waiter = new TaskCompletionSource<bool>();
                    Waiters.Enqueue(waiter);
                    return waiter.Task;
                }
            }
        }

        public void Release()
        {
            TaskCompletionSource<bool> toRelease = null;
            lock (Waiters)
            {
                if (Waiters.Count > 0)
                    toRelease = Waiters.Dequeue();
                else
                    ++CurrentCount;
            }
            toRelease?.SetResult(true);
        }

        #endregion
    }
}
