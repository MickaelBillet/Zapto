using Microsoft.Extensions.Configuration;
using WeatherZapto.Application.Infrastructure;
using WeatherZapto.Model;

namespace WeatherZapto.Infrastructure.WebServices
{
    internal class LocationService : WeatherZaptoWebService, ILocationService
    {
        #region Constructor
        public LocationService(IServiceProvider serviceProvider, IConfiguration configuration, string httpClientName) : base(serviceProvider, configuration, httpClientName)
        {
        }
        #endregion

        #region Methods
        public async Task<ZaptoLocation> GetReverseLocation(string longitude, string latitude)
        {
            return await this.WebService.GetAsync<ZaptoLocation>(string.Format(WeatherZaptoConstants.UrlReverseLocation, longitude, latitude),
                                                                                                null,
                                                                                                this.SerializerOptions,
                                                                                                new CancellationToken());
        }

        public async Task<IEnumerable<ZaptoLocation>> GetLocations(string city, string stateCode, string countryCode)
        {
            return await this.WebService.GetCollectionAsync<ZaptoLocation>(string.Format(WeatherZaptoConstants.UrlLocationCity, city, stateCode, countryCode),
                                                                                                this.SerializerOptions,
                                                                                                new CancellationToken());
        }

        public async Task<IEnumerable<ZaptoLocation>> GetLocations(string zipCode, string countryCode)
        {
            return await this.WebService.GetCollectionAsync<ZaptoLocation>(string.Format(WeatherZaptoConstants.UrlLocationZipCode, zipCode, countryCode),
                                                                                                this.SerializerOptions,
                                                                                                new CancellationToken());
        }
        #endregion
    }
}
