using Framework.Core.Base;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
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

    public partial class LocationSearchViewModel : BaseViewModel, ILocationSearchViewModel
    {
        private const int MinLength = 2;
        private const int ZipCodeSize = 5;

		#region Properties
		private IApplicationLocationService ApplicationLocationServices { get; }
        #endregion

        #region Constructor
        public LocationSearchViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
		{
            ApplicationLocationServices = serviceProvider.GetRequiredService<IApplicationLocationService>();
        }
        #endregion

        #region Methods

        [GeneratedRegex("^(\\d{5})\\s?,?([A-Z,a-z]{2})$")]
        private static partial Regex ZipCodeRegex();

        [GeneratedRegex(@"^([a-zA-Z\u0080-\u024F](?:[\-\'])?[a-zA-Z\u0080-\u024F]+)\s?([A-Z,a-z]{2})?$")]
        private static partial Regex CityNameRegx();

        public async Task<IEnumerable<LocationModel?>?> GetLocations(string input)
        {
            IEnumerable<LocationModel?>? output = null;

            try
            {
                IsLoading = true;

                if (string.IsNullOrEmpty(input) == false)
                {
                    Match? match = CityNameRegx().Match(input);
                    if (match.Success)
                    {
                        (string? city, string? countrycode)? result = ReadGroup(match.Groups);
                        if (result != null && result.Value.city!.Length > MinLength)
                        {
                            IEnumerable<ZaptoLocation> locations = await ApplicationLocationServices.GetLocations(result.Value.city, string.Empty, result.Value.countrycode);
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
                        match = ZipCodeRegex().Match(input);
                        if (match.Success)
                        {
                            (string? zipcode, string? countrycode)? result = ReadGroup(match.Groups);
                            if (result != null && result.Value.zipcode!.Length == ZipCodeSize)
                            {
                                ZaptoLocation location = await ApplicationLocationServices.GetLocation(result.Value.zipcode, result.Value.countrycode);
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
                Log.Debug($"{ClassHelper.GetCallerClassAndMethodName()} - {ex.ToString()}");
                this.NavigationService.ShowMessage("Location Service Exception", ZaptoSeverity.Error);
            }
            finally
            {
                IsLoading = false;
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
