using Framework.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using WeatherZapto.Application.Infrastructure;
using WeatherZapto.Model;

namespace WeatherZapto.Infrastructure.OpenWeatherServices
{
    internal class LocationOWService : ILocationOWService
    {
        #region Property
        protected IWebService WebService { get; private set; }
        protected IConfiguration Configuration { get; private set; }
        protected JsonSerializerOptions SerializerOptions { get; private set; }
        private const string LIMIT = "1";
        #endregion

        #region Constructor
        public LocationOWService(IServiceProvider serviceProvider, IConfiguration configuration, string httpClientName)
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

        #region Methods
        public async Task<IEnumerable<Location>> GetLocations(string APIKey, string longitude, string latitude)
        {
            return await this.WebService.GetCollectionAsync<Location>(string.Format(WeatherZaptoConstants.UrlOWLocation, latitude, longitude, LIMIT, APIKey),
                                                                                this.SerializerOptions,
                                                                                new CancellationToken()); ;
        }
        #endregion
    }
}
