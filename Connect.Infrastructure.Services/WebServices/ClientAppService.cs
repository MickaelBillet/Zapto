using Connect.Application.Infrastructure;
using Connect.Model;
using Microsoft.Extensions.Configuration;
using Serilog;
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

        public ClientAppService(IServiceProvider serviceProvider, IConfiguration configuration, string httpClientName) : base(serviceProvider, configuration, httpClientName)
        {

        }

        #endregion

        #region Method

        public async Task<bool?> AddClientAppAsync(ClientApp clientApp, CancellationToken token = default)
        {
            bool? res = false;

            try
            {
                res = await WebService.PostAsync<ClientApp>(ConnectConstants.RestUrlClientApps, clientApp, token);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

            return res;
        }

        public async Task<ClientApp?> GetClientAppAsync(string? firebaseToken, CancellationToken token = default)
        {
            ClientApp? res = null;

            try
            {
                res = await WebService.GetAsync<ClientApp>(ConnectConstants.RestUrlClientAppsToken, firebaseToken, SerializerOptions, token);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

            return res;
        }

        public async Task<bool?> DeleteClientAppAsync(string? firebaseToken, CancellationToken token = default)
        {
            bool? res = null;

            try
            {
                res = await WebService.DeleteAsync<ClientApp>(ConnectConstants.RestUrlClientAppsToken, firebaseToken, token);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

            return res;
        }

        #endregion
    }
}