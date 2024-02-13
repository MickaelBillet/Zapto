using System.Threading.Tasks;

namespace Framework.Infrastructure.Services
{
    public interface IStartupTask
    {
        Task Execute();
    }
}
