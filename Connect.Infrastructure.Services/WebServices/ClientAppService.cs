using Connect.Application.Infrastructure;
using Connect.Model;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Connect.Infrastructure.WebServices
{
    public class ClientAppService : ConnectWebService, IClientAppService
    {
        #region Property
        #endregion

        #region Constructor

        public ClientAppService(IServiceProvider serviceProvider, string httpClientName) : base(serviceProvider, httpClientName)
        {

        }

        #endregion

        #region Method

        public async Task<bool?> AddClientAppAsync(ClientApp clientApp, CancellationToken token = default)
        {
            return await WebService.PostAsync<ClientApp>(ConnectConstants.RestUrlClientApps, clientApp, token); ;
        }

        public async Task<ClientApp?> GetClientAppAsync(string? firebaseToken, CancellationToken token = default)
        {
            return await WebService.GetAsync<ClientApp>(ConnectConstants.RestUrlClientAppsToken, firebaseToken, SerializerOptions, token); ;
        }

        public async Task<bool?> DeleteClientAppAsync(string? firebaseToken, CancellationToken token = default)
        {
            return await WebService.DeleteAsync<ClientApp>(ConnectConstants.RestUrlClientAppsToken, firebaseToken, token); ;
        }

        #endregion
    }
}