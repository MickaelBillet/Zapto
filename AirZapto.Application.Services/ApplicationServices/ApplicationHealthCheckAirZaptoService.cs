using AirZapto.Application.Infrastructure;
using AirZapto.Model.Healthcheck;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace AirZapto.Application.Services
{
    internal class ApplicationHealthCheckAirZaptoService : IApplicationHealthCheckAirZaptoServices
    {
        #region Services
        private IHealthCheckAirZaptoService? HealthCheckService { get; }
        #endregion

        #region Constructor
        public ApplicationHealthCheckAirZaptoService(IServiceProvider serviceProvider)
        {
            this.HealthCheckService = serviceProvider.GetService<IHealthCheckAirZaptoService>();
        }
        #endregion

        #region Methods
        public async Task<HealthCheckAirZapto?> GetHealthCheckAirZapto()
        {
            return (this.HealthCheckService != null) ? await this.HealthCheckService.GetHealthCheckAirZapto() : null;
        }
        #endregion
    }
}
