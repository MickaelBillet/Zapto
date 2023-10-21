using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using WeatherZapto.Application.Infrastructure;
using WeatherZapto.Model.Healthcheck;

namespace WeatherZapto.Application.Services
{
    internal class ApplicationHealthChechWeatherZaptoService : IApplicationHealthCheckWeatherZaptoServices
    {
        #region Services
        private IHealthCheckWeatherZaptoService HealthCheckService { get; }
        #endregion

        #region Constructor
        public ApplicationHealthChechWeatherZaptoService(IServiceProvider serviceProvider)
        {
            this.HealthCheckService = serviceProvider.GetService<IHealthCheckWeatherZaptoService>();
        }
        #endregion

        #region Methods
        public async Task<HealthCheckWeatherZapto> GetHealthCheckWeatherZapto()
        {
            return (this.HealthCheckService != null) ? await this.HealthCheckService.GetHealthCheckWeatherZapto() : null;
        }
        #endregion
    }
}
