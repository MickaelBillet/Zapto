using Connect.Application;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
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
		public override async Task InitializeAsync(string? parameter)
		{
			await base.InitializeAsync(parameter);
		}
		public override void Dispose()
		{
			base.Dispose();
		}
		public async Task<LocationModel?> GetConnectLocationModel()
		{
			LocationModel? model = null;
			try
			{
				model = (await this.ApplicationConnectLocationServices.GetLocations())?.Select((location) => new LocationModel()
				{
					Name = location.City,
					Id = location.Id
				}).FirstOrDefault();
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
			}
			return model;
		}
        public async Task<LocationModel?> GetLocationModel(string latitude, string longitude)
        {
            LocationModel? model = null;
            try
            {
                ZaptoLocation location = await this.ApplicationLocationServices.GetLocation(longitude, latitude);
                model = (location != null) ? new LocationModel() { Name = location.Location } : null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return model;
        }
        public async Task TestNotification(string? locationId)
        {
            try
            {
                if (locationId != null)
                {
                    await this.ApplicationConnectLocationServices.TestNotication(locationId);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
        #endregion
    }
}
