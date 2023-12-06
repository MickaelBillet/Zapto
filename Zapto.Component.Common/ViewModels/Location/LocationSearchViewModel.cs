using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.Text.RegularExpressions;
using WeatherZapto.Application;
using WeatherZapto.Model;
using Zapto.Component.Common.Models;

namespace Zapto.Component.Common.ViewModels
{
    public interface ILocationSearchViewModel : IBaseViewModel
    {
        Task<IEnumerable<LocationModel?>?> GetLocations(string input);
    }

    public class LocationSearchViewModel : BaseViewModel, ILocationSearchViewModel
    {
        private const int MinLength = 2;
        private const int ZipCodeSize = 5;

		#region Properties
		private IApplicationLocationService ApplicationLocationServices { get; }
        #endregion

        #region Constructor
        public LocationSearchViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
		{
			this.ApplicationLocationServices = serviceProvider.GetRequiredService<IApplicationLocationService>();
        }
        #endregion

        #region Methods
        public async Task<IEnumerable<LocationModel?>?> GetLocations(string input)
        {
            IEnumerable<LocationModel?>? output = null;

            try
            { 
                this.IsLoading = true;

                if (string.IsNullOrEmpty(input) == false)
                {
                    Match? match = new Regex(@"^([a-zA-Z\u0080-\u024F](?:[\-\'])?[a-zA-Z\u0080-\u024F]+)\s?([A-Z,a-z]{2})?$").Match(input);
                    if (match.Success)
                    {
                        (string? city, string? countrycode)? result = this.ReadGroup(match.Groups);
                        if ((result != null) && (result.Value.city!.Length > MinLength))
                        {
                            IEnumerable<ZaptoLocation> locations = await this.ApplicationLocationServices.GetLocations(result.Value.city, string.Empty, result.Value.countrycode);
                            if (locations != null)
                            {
                                output = locations.GroupBy(x => new { x.Location, x.State, x.Country, x.Latitude, x.Longitude })
                                                    .Select(y => y.First())
                                                    .Select(x => new LocationModel()
                                                    {
                                                        Country = x.Country,
                                                        Longitude = x.Longitude,
                                                        Location = x.Location,
                                                        Latitude = x.Latitude,
                                                        State = x.State,
                                                    });
                            }
                        }
                    }
                    else
                    {
                        match = new Regex(@"^(\d{5})\s?,?([A-Z,a-z]{2})$").Match(input);
                        if (match.Success)
                        {
                            (string? zipcode, string? countrycode)? result = this.ReadGroup(match.Groups);
                            if ((result != null) && (result.Value.zipcode!.Length == ZipCodeSize))
                            {
                                ZaptoLocation location = await this.ApplicationLocationServices.GetLocation(result.Value.zipcode, result.Value.countrycode);
                                if (location != null) 
                                {
                                    output = new List<LocationModel>
                                    {
                                        new LocationModel()
                                        {
                                            Country = location.Country,
                                            Longitude = location.Longitude,
                                            Location = location.Location,
                                            Latitude = location.Latitude,
                                            Zip = location.Zip,
                                        },
                                    };
                                }
                            }
                        }
                    }
                }
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

            return output;
        }

        private (string? value1, string? value2)? ReadGroup(GroupCollection groupCollection)
        {
            (string? value1, string? value2)? output = null;            
            if (groupCollection != null)
            {
                output = new()
                {
                    value1 = groupCollection[1].Value,
                    value2 = groupCollection[2].Value,
                };
            }
            return output;
        }
        #endregion
    }
}
