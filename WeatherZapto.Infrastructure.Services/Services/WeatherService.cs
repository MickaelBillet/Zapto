using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;
using WeatherZapto.Application.Infrastructure;
using WeatherZapto.Model;

namespace WeatherZapto.Infrastructure.WebServices
{
    public class WeatherService : WeatherZaptoWebService, IWeatherService
    {
        #region Constructor
        public WeatherService(IServiceProvider serviceProvider, IConfiguration configuration, string httpClientName) : base(serviceProvider, configuration, httpClientName)
		{

        }
		#endregion

		#region Method
		public async Task<ZaptoWeather> GetWeather(string longitude, string latitude, string culture)
		{
			return await this.WebService.GetAsync<ZaptoWeather>(string.Format(WeatherZaptoConstants.UrlWeather, longitude, latitude, culture),
                                                                        null,
                                                                        this.SerializerOptions,
                                                                        new CancellationToken()); ;
		}

        public async Task<ZaptoWeather> GetWeather(string location, string longitude, string latitude, string culture)
        {
            return await this.WebService.GetAsync<ZaptoWeather>(string.Format(WeatherZaptoConstants.UrlWeatherLocation, location, longitude, latitude, culture),
                                                                        null,
                                                                        this.SerializerOptions,
                                                                        new CancellationToken());
        }
        #endregion
    }
}
