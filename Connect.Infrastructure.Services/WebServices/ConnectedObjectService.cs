using Connect.Application.Infrastructure;
using Connect.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace Connect.Infrastructure.WebServices
{
    public class ConnectedObjectService : ConnectWebService, IConnectedObjectService
    {
        #region Property


        #endregion

        #region Constructor

        public ConnectedObjectService(IServiceProvider serviceProvider, IConfiguration configuration, string httpClientName) : base(serviceProvider, configuration, httpClientName)
        {
        }

        #endregion

        #region Method

        public async Task<ConnectedObject?> GetConnectedObject(string objectId)
        {
            return await WebService.GetAsync<ConnectedObject>(ConnectConstants.RestUrlConnectedObjectId, objectId, SerializerOptions); ;
        }

        #endregion
    }
}
