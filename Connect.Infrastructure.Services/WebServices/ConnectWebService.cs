using Framework.Infrastructure.Services;
using System;
using System.Text.Json;

namespace Connect.Infrastructure.WebServices
{
    public abstract class ConnectWebService
    {
        #region Property
        protected IWebService WebService { get; private set; }
        protected JsonSerializerOptions SerializerOptions { get; private set; }
        #endregion

        #region Constructor
        public ConnectWebService(IServiceProvider serviceProvider, string httpClientName)
        {
            this.SerializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,
            };

            this.WebService = WebServiceFactory.CreateWebService(serviceProvider, httpClientName);
        }
        #endregion
    }
}
