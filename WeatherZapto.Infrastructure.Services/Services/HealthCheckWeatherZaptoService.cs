using WeatherZapto.Application.Infrastructure;
using WeatherZapto.Model.Healthcheck;

namespace WeatherZapto.Infrastructure.WebServices
{
    internal class HealthCheckWeatherZaptoService : WeatherZaptoWebService, IHealthCheckWeatherZaptoService
    {
        #region Constructor
        public HealthCheckWeatherZaptoService(IServiceProvider serviceProvider, string httpClientName) : base(serviceProvider, httpClientName)
        { 

        }
        #endregion

        #region Method  
        public async Task<HealthCheckWeatherZapto> GetHealthCheckWeatherZapto(CancellationToken token = default)
        {
            return await WebService.GetAsync<HealthCheckWeatherZapto>(WeatherZaptoConstants.UrlHealthCheck, null, null, token); ;
        }
        #endregion
    }
}
