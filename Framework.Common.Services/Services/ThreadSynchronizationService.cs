using System.Collections.Generic;
using System.Threading;

namespace Framework.Infrastructure.Services
{
    internal class ThreadSynchronizationService : IThreadSynchronizationService
    {
        private static Dictionary<string, ManualResetEvent> DictionnaryMre { get; } = new Dictionary<string, ManualResetEvent>();

        public ThreadSynchronizationService() { }

        public bool WaitOne(string name)
        {
            ManualResetEvent manualResetEvent = DictionnaryMre.GetValueOrDefault(name);
            if (manualResetEvent == null)
            {
                manualResetEvent = new ManualResetEvent(false);
                DictionnaryMre.Add(name, manualResetEvent);
            }
            return manualResetEvent.WaitOne();
        }

        public bool Set(string name)
        {
            ManualResetEvent manualResetEvent = DictionnaryMre.GetValueOrDefault(name);
            if (manualResetEvent == null)
            {
                manualResetEvent = new ManualResetEvent(false);
                DictionnaryMre.Add(name, manualResetEvent);
            }
            return manualResetEvent.Set();
        }
    }
}
