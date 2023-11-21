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
        public async Task<ZaptoLocation> GetLocation(string longitude, string latitude)
        {
            return (this.LocationService != null) ? await this.LocationService.GetLocation(longitude, latitude) : null;
        }
        #endregion
    }
}
