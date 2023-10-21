using AirZapto.Model.Healthcheck;
using System.Threading;
using System.Threading.Tasks;

namespace AirZapto.Application.Infrastructure
{
    public interface IHealthCheckAirZaptoService
    {
        Task<HealthCheckAirZapto?> GetHealthCheckAirZapto(CancellationToken token = default);
    }
}
