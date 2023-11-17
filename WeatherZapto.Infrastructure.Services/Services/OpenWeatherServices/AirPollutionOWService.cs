using Framework.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using WeatherZapto.Application.Infrastructure;
using WeatherZapto.Model;

namespace WeatherZapto.Infrastructure.OpenWeatherServices
{
    internal class AirPollutionOWService : IAirPollutionOWService
    {
        #region Property
        protected IWebService WebService { get; private set; }
        protected IConfiguration Configuration { get; private set; }
        protected JsonSerializerOptions SerializerOptions { get; private set; }
        #endregion

        #region Constructor
        public AirPollutionOWService(IServiceProvider serviceProvider, IConfiguration configuration, string httpClientName)
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
        public async Task<AirPollution> GetAirPollution(string APIKey, string longitude, string latitude)
        {
            return await this.WebService.GetAsync<AirPollution>(string.Format(WeatherZaptoConstants.UrlOWAirPollutionCurrent, latitude, longitude, APIKey),
                                                                                                    null,
                                                                                                    this.SerializerOptions,
                                                                                                    new CancellationToken()); ;
        }
        #endregion
    }
}
