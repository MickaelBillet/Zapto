using Authentication.Application;
using Authentication.Application.Infrastructure;
using Authentication.Model;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace AirZapto.Application.Services
{
    internal class ApplicationOpenIdConfigurationService : IApplicationOpenIdConfigurationServices
    {
        #region Services
        private IOpenIdConfigurationService? OpenIdConfigurationService { get; }
        #endregion

        #region Constructor
        public ApplicationOpenIdConfigurationService(IServiceProvider serviceProvider)
        {
            this.OpenIdConfigurationService = serviceProvider.GetRequiredService<IOpenIdConfigurationService>();
        }
        #endregion

        #region Methods
        public async Task<OpenIdConfiguration?> GetOpenIdConfiguration()
        {
            return (this.OpenIdConfigurationService != null) ? await this.OpenIdConfigurationService.GetOpenIdConfiguration() : null;
        }
        #endregion
    }
}
