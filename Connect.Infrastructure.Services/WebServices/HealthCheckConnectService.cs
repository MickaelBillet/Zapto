using Connect.Application.Infrastructure;
using Connect.Model;
using Connect.Model.Healthcheck;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Connect.Infrastructure.WebServices
{
    internal class HealthCheckConnectService : ConnectWebService, IHealthCheckConnectService
    {
        #region Constructor
        public HealthCheckConnectService(IServiceProvider serviceProvider, string httpClientName) : base(serviceProvider, httpClientName)
        {

        }
        #endregion

        #region Method  
        public async Task<HealthCheckConnect?> GetHealthCheckConnect(CancellationToken token = default)
        {
            return await WebService.GetAsync<HealthCheckConnect>(ConnectConstants.RestUrlHealthCheck, null, null, token);
        }
        #endregion
    }
}
