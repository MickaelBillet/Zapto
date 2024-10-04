using Connect.Application.Infrastructure;
using Connect.Model;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Connect.Application.Services
{
    internal sealed class ApplicationClientAppServices : IApplicationClientAppServices
    {
        #region Services
        public IClientAppService? ClientAppService { get; }
        #endregion

        #region Constructor

        public ApplicationClientAppServices(IServiceProvider serviceProvider)
        {
            this.ClientAppService = serviceProvider.GetService<IClientAppService>();
        }

        #endregion

        #region Methods
        public async Task<bool?> Save(ClientApp clientApp)
        {
            return (this.ClientAppService != null) ? await this.ClientAppService.AddClientAppAsync(clientApp) : null;
        }

        public async Task<ClientApp?> GetClientAppFromToken(string token)
        {
            return (this.ClientAppService != null) ? await this.ClientAppService.GetClientAppAsync(token) : null;
        }

        public async Task<bool?> Delete(string token)
        {
            return (this.ClientAppService != null) ? await this.ClientAppService.DeleteClientAppAsync(token) : null;
        }
        #endregion
    }
}