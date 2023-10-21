using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Threading;
using System.Threading.Tasks;
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
            ZaptoLocation location = null;

            try
            {
                location = await this.WebService.GetAsync<ZaptoLocation>(string.Format(WeatherZaptoConstants.RestUrlLocation, longitude, latitude),
                                                                                                null,
                                                                                                this.SerializerOptions,
                                                                                                new CancellationToken());
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

            return location;
        }
        #endregion
    }
}
