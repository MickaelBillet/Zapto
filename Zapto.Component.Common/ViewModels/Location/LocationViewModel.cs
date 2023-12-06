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
        Task<string?> GetReverseLocation(string latitude, string longitude);
        Task TestNotification(string? locationId);
        Task<LocationModel?> GetLocationModel();
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
        public async Task<string?> GetReverseLocation(string latitude, string longitude)
        {
            ZaptoLocation? zaptoLocation = null;
            try
            {
                this.IsLoading = true;
                zaptoLocation = await this.ApplicationLocationServices.GetReverseLocation(longitude, latitude);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw new Exception("Location Service Exception : " + ex.Message);
            }
            finally
            {
                this.IsLoading = false;
            }
            return zaptoLocation?.Location;
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

        public async Task<LocationModel?> GetLocationModel()
        {
            LocationModel? model = null;
            try
            {
                this.IsLoading = true;

                model = (await this.ApplicationConnectLocationServices.GetLocations())?.Select((location) => new LocationModel()
                {
                    Location = location.City,
                    Id = location.Id
                }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw new Exception("Location Service Exception : " + ex.Message);
            }
            finally
            {
                this.IsLoading = false;
            }
            return model;
        }
        #endregion
    }
}
