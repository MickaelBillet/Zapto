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
        Task<LocationModel?> GetLocationModel(string latitude, string longitude);
        Task TestNotification(string? locationId);
    }

    public class LocationViewModel : BaseViewModel, ILocationViewModel
	{
		#region Properties
		private IApplicationLocationService ApplicationLocationServices { get; }
        private IApplicationConnectLocationServices ApplicationConnectLocationServices { get; }
        #endregion

        #region Constructor
        public LocationViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
		{
			this.ApplicationLocationServices = serviceProvider.GetRequiredService<IApplicationLocationService>();
            this.ApplicationConnectLocationServices = serviceProvider.GetRequiredService<IApplicationConnectLocationServices>();
        }
        #endregion

        #region Methods
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
                throw new Exception("Location Service Exception : " + ex.Message);
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
