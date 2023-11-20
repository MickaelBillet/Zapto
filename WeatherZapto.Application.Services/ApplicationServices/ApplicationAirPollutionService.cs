using Microsoft.Extensions.DependencyInjection;
using WeatherZapto.Application.Infrastructure;
using WeatherZapto.Model;

namespace WeatherZapto.Application.Services
{
    internal class ApplicationAirPollutionService : IApplicationAirPollutionService
	{
		#region Services
        private IAirPollutionService AirPollutionService { get; }
        #endregion

        #region Constructor
        public ApplicationAirPollutionService(IServiceProvider serviceProvider)
		{
            this.AirPollutionService = serviceProvider.GetService<IAirPollutionService>();
		}
		#endregion

		#region Methods
        public async Task<ZaptoAirPollution> GetCurrentAirPollution(string locationName, string longitude, string latitude)
        {
            return (this.AirPollutionService != null) ? await this.AirPollutionService.GetAirPollution(locationName, longitude, latitude) : null;
        }

        public async Task<ZaptoAirPollution> GetCurrentAirPollution(string longitude, string latitude)
        {
            return (this.AirPollutionService != null) ? await this.AirPollutionService.GetAirPollution(longitude, latitude) : null;
        }
        #endregion
    }
}
