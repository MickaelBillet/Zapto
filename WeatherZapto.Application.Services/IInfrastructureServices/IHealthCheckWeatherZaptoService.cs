using System.Threading;
using System.Threading.Tasks;
using WeatherZapto.Model.Healthcheck;

namespace WeatherZapto.Application.Infrastructure
{
    public interface IHealthCheckWeatherZaptoService
    {
        Task<HealthCheckWeatherZapto> GetHealthCheckWeatherZapto(CancellationToken token = default);
    }
}
