using Framework.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text.Json;

namespace AirZapto.Infrastructure.WebServices
{
    internal abstract class AirZaptoWebService
	{
        #region Property
        protected IWebService WebService { get; private set; }
        protected IConfiguration Configuration { get; private set; }
		protected JsonSerializerOptions SerializerOptions { get; private set; }
        #endregion

        #region Constructor
        public AirZaptoWebService(IServiceProvider serviceProvider, IConfiguration configuration, string httpClientName)
		{
            this.WebService = WebServiceFactory.CreateWebService(serviceProvider, httpClientName);
            this.Configuration = configuration;
            this.SerializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,
            };
        }
		#endregion
	}
}
