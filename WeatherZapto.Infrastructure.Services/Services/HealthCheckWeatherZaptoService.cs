using Microsoft.Extensions.Configuration;
using Serilog;
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
            HealthCheckWeatherZapto healthCheck = null;
            try
            {
                healthCheck = await WebService.GetAsync<HealthCheckWeatherZapto>(WeatherZaptoConstants.RestUrlHealthCheck, null, null, token);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

            return healthCheck;
        }
        #endregion
    }
}
