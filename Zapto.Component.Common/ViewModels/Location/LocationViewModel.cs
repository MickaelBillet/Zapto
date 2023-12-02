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
        Task<IEnumerable<string?>>? GetLocations(string input);
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

        public async Task<IEnumerable<string?>>? GetLocations(string input)
        {
            IEnumerable<string?>? output = null;

            Regex regex = new Regex(@"^([a-zA-Z\u0080-\u024F](?:[\-\'])?[a-zA-Z\u0080-\u024F]+)\s?([A-Z,a-z]{2})?$");
            Match? match = regex.Match(input);
            if (match.Success)
            {
                (string? city, string? countrycode)? city = this.ReadGroupCity(match.Groups);
                if ((city != null) && (city.Value.city!.Length > 1))
                {
                    IEnumerable<ZaptoLocation> locations = await this.ApplicationLocationServices.GetLocations(city.Value.city, string.Empty, city.Value.countrycode);
                    output = locations.GroupBy(x => new
                    { x.Location, x.State, x.Country, x.Latitude, x.Longitude }).Select(y => y.First()).Select(x => x.Location + " " + x.State + " " + x.Country + " " + x.Latitude.ToString("F3") + " " + x.Longitude.ToString("F3"));
                }
            }
            else
            {
                regex = new Regex(@"^(\d{5})\s?,?([A-Z,a-z]{2})?$");
                match = regex.Match(input);
                if (match.Success)
                {
                    (string? zipcode, string? countrycode)? zipcode = this.ReadGroupZipCode(match.Groups);
                    if (zipcode != null)
                    {
                        IEnumerable<ZaptoLocation> locations = await this.ApplicationLocationServices.GetLocations(zipcode.Value.zipcode, zipcode.Value.countrycode);
                        output = locations.Select(x => x.Location + " " + x.Country);
                    }
                }
            }

            return output;
        }

        private (string? city, string? countrycode)? ReadGroupCity(GroupCollection groupCollection)
        {
            (string? city, string? countrycode)? output = null;            
            if (groupCollection != null)
            {
                output = new()
                {
                    city = groupCollection[1].Value,
                    countrycode = groupCollection[2].Value,
                };
            }
            return output;
        }

        private (string? zipcode, string ? countrycode)? ReadGroupZipCode(GroupCollection groupCollection)
        {
            (string? zipcode, string? countrycode)? output = null;
            if (groupCollection != null)
            {
                output = new()
                {
                    zipcode = groupCollection[1].Value,
                    countrycode = groupCollection[2].Value,
                };
            }
            return output;
        }

        public async Task<LocationModel?> GetLocationModel(string latitude, string longitude)
        {
            LocationModel? model = null;
            try
            {
                ZaptoLocation location = await this.ApplicationLocationServices.GeReversetLocation(longitude, latitude);
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
