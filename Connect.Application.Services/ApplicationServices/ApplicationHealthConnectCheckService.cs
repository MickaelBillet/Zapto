using Connect.Application.Infrastructure;
using Connect.Model.Healthcheck;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Connect.Application.Services
{
    internal sealed class ApplicationHealthConnectCheckService : IApplicationHealthCheckConnectServices
    {
        #region Services
        private IHealthCheckConnectService? HealthCheckService { get; }
        #endregion

        #region Constructor
        public ApplicationHealthConnectCheckService(IServiceProvider serviceProvider)
        {
            this.HealthCheckService = serviceProvider.GetService<IHealthCheckConnectService>();
        }
        #endregion

        #region Methods
        public async Task<HealthCheckConnect?> GetHealthCheckConnect()
        {
            return (this.HealthCheckService != null) ? await this.HealthCheckService.GetHealthCheckConnect() : null; ;  
        }
        #endregion
    }
}
