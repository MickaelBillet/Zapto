using WeatherZapto.Application.Infrastructure;
using WeatherZapto.Model;

namespace WeatherZapto.Infrastructure.WebServices
{
    internal class AirPollutionService : WeatherZaptoWebService, IAirPollutionService
    {
        #region Constructor
        public AirPollutionService(IServiceProvider serviceProvider, string httpClientName) : base(serviceProvider, httpClientName)
        {

        }
        #endregion

        #region Methods
        public async Task<ZaptoAirPollution> GetAirPollution(string longitude, string latitude)
        {
            return await this.WebService.GetAsync<ZaptoAirPollution>(string.Format(WeatherZaptoConstants.UrlAirPollution, longitude, latitude),
                                                                                                    null,
                                                                                                    this.SerializerOptions,
                                                                                                    new CancellationToken()); ;
        }

        public async Task<ZaptoAirPollution> GetAirPollution(string location, string longitude, string latitude)
        {
            return await this.WebService.GetAsync<ZaptoAirPollution>(string.Format(WeatherZaptoConstants.UrlAirPollutionLocation, location, longitude, latitude),
                                                                                                    null,
                                                                                                    this.SerializerOptions,
                                                                                                    new CancellationToken()); ;
        }
        #endregion
    }
}
