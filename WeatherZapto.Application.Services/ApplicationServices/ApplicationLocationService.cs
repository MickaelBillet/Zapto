using Microsoft.Extensions.DependencyInjection;
using WeatherZapto.Application.Infrastructure;
using WeatherZapto.Model;

namespace WeatherZapto.Application.Services
{
    internal class ApplicationLocationService : IApplicationLocationService
	{
		#region Services
		private ILocationService LocationService { get; }
		#endregion

		#region Constructor
		public ApplicationLocationService(IServiceProvider serviceProvider)
		{
			this.LocationService = serviceProvider.GetService<ILocationService>();
        }
		#endregion

		#region Methods
        public async Task<ZaptoLocation> GeReversetLocation(string longitude, string latitude)
        {
            return (this.LocationService != null) ? await this.LocationService.GetReverseLocation(longitude, latitude) : null;
        }

        public async Task<IEnumerable<ZaptoLocation>> GetLocations(string city, string stateCode, string countryCode)
        {
            return (this.LocationService != null) ? await this.LocationService.GetLocations(city, countryCode, stateCode) : null;
        }

        public async Task<IEnumerable<ZaptoLocation>> GetLocations(string zipCode, string countryCode)
        {
            return (this.LocationService != null) ? await this.LocationService.GetLocations(zipCode, countryCode) : null;
        }
        #endregion
    }
}
