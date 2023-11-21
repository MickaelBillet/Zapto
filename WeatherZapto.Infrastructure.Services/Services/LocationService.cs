using Microsoft.Extensions.Configuration;
using WeatherZapto.Application.Infrastructure;
using WeatherZapto.Model;

namespace WeatherZapto.Infrastructure.WebServices
{
    internal class LocationService :  WeatherZaptoWebService, ILocationService
    {
        #region Constructor
        public LocationService(IServiceProvider serviceProvider, IConfiguration configuration, string httpClientName): base (serviceProvider, configuration, httpClientName)
        {
        }
        #endregion

        #region Methods
        public async Task<ZaptoLocation> GetLocation(string longitude, string latitude)
        {
            return await this.WebService.GetAsync<ZaptoLocation>(string.Format(WeatherZaptoConstants.RestUrlLocation, longitude, latitude),
                                                                                                null,
                                                                                                this.SerializerOptions,
                                                                                                new CancellationToken());
        }
        #endregion
    }
}
