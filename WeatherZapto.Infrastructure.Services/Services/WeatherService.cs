using Microsoft.Extensions.Configuration;
using Serilog;
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
            ZaptoWeather current = null;

			try
			{
				current = await this.WebService.GetAsync<ZaptoWeather>(string.Format(WeatherZaptoConstants.RestUrlWeather, longitude, latitude, culture), 
																		null, 
																		this.SerializerOptions, 
																		new CancellationToken());
			}
			catch (Exception ex)
			{
				Log.Error(ex.Message);
			}

			return current;
		}

        public async Task<ZaptoWeather> GetWeather(string location, string longitude, string latitude, string culture)
        {
            ZaptoWeather current = null;

            try
            {
                current = await this.WebService.GetAsync<ZaptoWeather>(string.Format(WeatherZaptoConstants.RestUrlWeatherLocation, location, longitude, latitude, culture),
                                                                        null,
                                                                        this.SerializerOptions,
                                                                        new CancellationToken());
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

            return current;
        }
        #endregion
    }
}
