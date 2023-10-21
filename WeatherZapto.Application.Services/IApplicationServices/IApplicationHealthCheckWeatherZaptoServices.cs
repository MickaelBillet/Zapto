using System.Threading.Tasks;
using WeatherZapto.Model.Healthcheck;

namespace WeatherZapto.Application
{
    public interface IApplicationHealthCheckWeatherZaptoServices
	{
        Task<HealthCheckWeatherZapto> GetHealthCheckWeatherZapto();
    }
}
