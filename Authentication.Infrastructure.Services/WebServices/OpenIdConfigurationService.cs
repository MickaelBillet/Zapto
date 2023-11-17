using Authentication;
using Authentication.Application.Infrastructure;
using Authentication.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AirZapto.Infrastructure.WebServices
{
    internal sealed class OpenIdConfigurationService : AuthenticationWebService, IOpenIdConfigurationService
    {
        #region Constructor
        public OpenIdConfigurationService(IServiceProvider serviceProvider, IConfiguration configuration, string httpClientName) : base(serviceProvider, configuration, httpClientName)
        {
        }
        #endregion

        #region Method  
        public async Task<OpenIdConfiguration?> GetOpenIdConfiguration(CancellationToken token = default)
        {
            return await this.WebService.GetAsync<OpenIdConfiguration>(AuthenticationConstants.UrlOpenidConfiguration, null, null, token);
        }
        #endregion
    }
}
