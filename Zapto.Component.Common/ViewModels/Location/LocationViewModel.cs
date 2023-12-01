using Connect.Application;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.Text.RegularExpressions;
using WeatherZapto.Application;
using WeatherZapto.Model;
using Zapto.Component.Common.Models;

namespace Zapto.Component.Common.ViewModels
{
    public interface ILocationViewModel : IBaseViewModel
    {
        Task<LocationModel?> GetLocationModel(string latitude, string longitude);
        Task TestNotification(string? locationId);
        Task<LocationModel?> GetLocationModel();
        IEnumerable<string>? GetLocations(string input);
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

        public IEnumerable<string>? GetLocations(string input)
        {
            IEnumerable<string>? output = null;

            Regex regex = new Regex(@"^(([A-Z][a-z]+\s?)+,?\s?([A-Z,a-z]{0,2})?\s?(\d{5})?)$");
            Match? match = regex.Match(input);
            if (match.Success)
            {
                output = match.Groups.Values.Select(item => item.Value);
            }
            else
            {
                regex = new Regex(@"^((\d{5})\s?,?([A-Z,a-z]{0,2})?)$");
                match = regex.Match(input);
                if (match.Success)
                {
                    output = match.Groups.Values.Select(item => item.Value);
                }
            }

            return output;
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

        public async Task<LocationModel?> GetLocationModel()
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
                throw new Exception("Location Service Exception : " + ex.Message);
            }
            return model;
        }
        #endregion
    }
}
