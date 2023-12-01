using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;
using WeatherZapto.Application.Infrastructure;
using WeatherZapto.Model.Healthcheck;

namespace WeatherZapto.Infrastructure.WebServices
{
    internal class HealthCheckWeatherZaptoService : WeatherZaptoWebService, IHealthCheckWeatherZaptoService
    {
        #region Constructor
        public HealthCheckWeatherZaptoService(IServiceProvider serviceProvider, IConfiguration configuration, string httpClientName) : base(serviceProvider, configuration, httpClientName)
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
