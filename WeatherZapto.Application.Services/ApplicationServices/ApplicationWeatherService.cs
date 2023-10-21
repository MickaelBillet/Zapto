using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using WeatherZapto.Application.Infrastructure;
using WeatherZapto.Model;

namespace WeatherZapto.Application.Services
{
    internal class ApplicationWeatherService : IApplicationWeatherService
	{
		#region Services
        private IWeatherService WeatherService { get; }
        #endregion

        #region Constructor
        public ApplicationWeatherService(IServiceProvider serviceProvider)
		{
            this.WeatherService = serviceProvider.GetService<IWeatherService>();
        }
        #endregion

        #region Methods
        public async Task<ZaptoWeather> GetCurrentWeather(string locationName, string longitude, string latitude, string culture)
        {
            return (this.WeatherService != null) ? await this.WeatherService.GetWeather(locationName, longitude, latitude, culture) : null;
        }
        public async Task<ZaptoWeather> GetCurrentWeather(string longitude, string latitude, string culture)
        {
            return (this.WeatherService != null) ? await this.WeatherService.GetWeather(longitude, latitude, culture) : null;
        }
		#endregion
	}
}
