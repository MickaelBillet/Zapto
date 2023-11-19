using Connect.Application;
using Microsoft.Extensions.DependencyInjection;
using WeatherZapto.Application;
using WeatherZapto.Model;
using Zapto.Component.Common.Models;

namespace Zapto.Component.Common.ViewModels
{
    public interface ILocationViewModel : IBaseViewModel
    {
		Task<LocationModel?> GetConnectLocationModel();
        Task<LocationModel?> GetLocationModel(string latitude, string longitude);
        Task TestNotification(string? locationId);
    }

    public class LocationViewModel : BaseViewModel, ILocationViewModel
	{
		#region Properties
		private IApplicationConnectLocationServices ApplicationConnectLocationServices { get; }
		private IApplicationLocationService ApplicationLocationServices { get; }
		#endregion

		#region Constructor
		public LocationViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
		{
			this.ApplicationConnectLocationServices = serviceProvider.GetRequiredService<IApplicationConnectLocationServices>();
			this.ApplicationLocationServices = serviceProvider.GetRequiredService<IApplicationLocationService>();
		}
		#endregion

		#region Methods

		public async Task<LocationModel?> GetConnectLocationModel()
		{
			return (await this.ApplicationConnectLocationServices.GetLocations())?.Select((location) => new LocationModel()
            {
                Name = location.City,
                Id = location.Id
            }).FirstOrDefault(); ;
		}
        public async Task<LocationModel?> GetLocationModel(string latitude, string longitude)
        {
            ZaptoLocation location = await this.ApplicationLocationServices.GetLocation(longitude, latitude);
            return (location != null) ? new LocationModel() { Name = location.Location } : null; ;
        }
        public async Task TestNotification(string? locationId)
        {
            if (locationId != null)
            {
                await this.ApplicationConnectLocationServices.TestNotication(locationId);
            }
        }
        #endregion
    }
}
