using Connect.Application.Infrastructure;
using Connect.Model;
using Connect.Model.Healthcheck;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Connect.Infrastructure.WebServices
{
    internal class HealthCheckConnectService : ConnectWebService, IHealthCheckConnectService
    {
        #region Constructor
        public HealthCheckConnectService(IServiceProvider serviceProvider, IConfiguration configuration, string httpClientName) : base(serviceProvider, configuration, httpClientName)
        {

        }
        #endregion

        #region Method  
        public async Task<HealthCheckConnect?> GetHealthCheckConnect(CancellationToken token = default)
        {
            HealthCheckConnect? healthCheck = null;
            try
            {
                healthCheck = await WebService.GetAsync<HealthCheckConnect>(ConnectConstants.RestUrlHealthCheck, null, null, token);
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
