namespace Framework.Infrastructure.Services
{
    public interface IThreadSynchronizationService
    {
        bool Set(string name);
        bool WaitOne(string name);
    }
}