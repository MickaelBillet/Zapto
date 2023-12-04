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
            if (string.IsNullOrEmpty(countryCode) == false)
            {
                return await this.WebService.GetCollectionAsync<ZaptoLocation>(string.Format(WeatherZaptoConstants.UrlLocationCity2, city, countryCode),
                                                                                                this.SerializerOptions,
                                                                                                new CancellationToken());
            }
            else
            {
                return await this.WebService.GetCollectionAsync<ZaptoLocation>(string.Format(WeatherZaptoConstants.UrlLocationCity1, city),
                                                                                                this.SerializerOptions,
                                                                                                new CancellationToken());
            }
        }

        public async Task<ZaptoLocation> GetLocation(string zipCode, string countryCode)
        {
            return await this.WebService.GetAsync<ZaptoLocation>(string.Format(WeatherZaptoConstants.UrlLocationZipCode, zipCode, countryCode),
                                                                                                null,
                                                                                                this.SerializerOptions,
                                                                                                new CancellationToken());
        }
        #endregion
    }
}
