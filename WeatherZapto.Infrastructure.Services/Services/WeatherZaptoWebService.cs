using Framework.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Text.Json;

namespace WeatherZapto.Infrastructure.WebServices
{
    public abstract class WeatherZaptoWebService
    {
        #region Property
        protected IWebService WebService { get; private set; }
        protected JsonSerializerOptions SerializerOptions { get; private set; }
        #endregion

        #region Constructor
        public WeatherZaptoWebService(IServiceProvider serviceProvider, IConfiguration configuration, string httpClientName)
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
