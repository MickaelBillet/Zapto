﻿using Framework.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
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
        private const string LIMIT = "10";
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
        public async Task<IEnumerable<Location>> GetReverseLocations(string APIKey, string longitude, string latitude)
        {
            return await this.WebService.GetCollectionAsync<Location>(string.Format(WeatherZaptoConstants.UrlOWReverseLocation, latitude, longitude, LIMIT, APIKey),
                                                                                            this.SerializerOptions,
                                                                                            new CancellationToken());
        }
        public async Task<IEnumerable<Location>> GetLocationsFromCity(string APIKey, string city, string stateCode, string countryCode)
        {
            return await this.WebService.GetCollectionAsync<Location>(string.Format(WeatherZaptoConstants.UrlOWLocationCity, city, stateCode, countryCode, LIMIT, APIKey),
                                                                                            this.SerializerOptions,
                                                                                            new CancellationToken());
        }
        public async Task<Location> GetLocationFromZipCode(string APIKey, string zipCode, string countryCode)
        {
            return await this.WebService.GetAsync<Location>(string.Format(WeatherZaptoConstants.UrlOWLocationZipCode, zipCode, countryCode, APIKey),    
                                                                                            null,
                                                                                            this.SerializerOptions,
                                                                                            new CancellationToken());
        }
        #endregion
    }
}
