using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Threading;
using System.Threading.Tasks;
using WeatherZapto.Application.Infrastructure;
using WeatherZapto.Model;

namespace WeatherZapto.Infrastructure.WebServices
{
    internal class AirPollutionService : WeatherZaptoWebService, IAirPollutionService
    {
        #region Constructor
        public AirPollutionService(IServiceProvider serviceProvider, IConfiguration configuration, string httpClientName) : base(serviceProvider, configuration, httpClientName)
        {

        }
        #endregion

        #region Methods
        public async Task<ZaptoAirPollution> GetAirPollution(string longitude, string latitude)
        {
            ZaptoAirPollution airPollution = null;

            try
            {
                airPollution = await this.WebService.GetAsync<ZaptoAirPollution>(string.Format(WeatherZaptoConstants.RestUrlAirPollution, longitude, latitude),
                                                                                                    null,
                                                                                                    this.SerializerOptions,
                                                                                                    new CancellationToken());
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

            return airPollution;
        }

        public async Task<ZaptoAirPollution> GetAirPollution(string location, string longitude, string latitude)
        {
            ZaptoAirPollution airPollution = null;

            try
            {
                airPollution = await this.WebService.GetAsync<ZaptoAirPollution>(string.Format(WeatherZaptoConstants.RestUrlAirPollutionLocation, location, longitude, latitude),
                                                                                                    null,
                                                                                                    this.SerializerOptions,
                                                                                                    new CancellationToken());
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

            return airPollution;
        }
        #endregion
    }
}
