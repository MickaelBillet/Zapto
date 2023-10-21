using Connect.Model.Healthcheck;
using System.Threading;
using System.Threading.Tasks;

namespace Connect.Application.Infrastructure
{
    public interface IHealthCheckConnectService
    {
        Task<HealthCheckConnect?> GetHealthCheckConnect(CancellationToken token = default);
    }
}
