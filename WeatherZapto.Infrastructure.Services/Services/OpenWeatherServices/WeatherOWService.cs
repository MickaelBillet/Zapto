using Framework.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using WeatherZapto.Application.Infrastructure;
using WeatherZapto.Model;

namespace WeatherZapto.Infrastructure.OpenWeatherServices
{
    internal class WeatherOWService : IWeatherOWService
	{
        #region Property
        protected IWebService WebService { get; private set; }
        protected IConfiguration Configuration { get; private set; }
        protected JsonSerializerOptions SerializerOptions { get; private set; }
        #endregion

        #region Constructor

        public WeatherOWService(IServiceProvider serviceProvider, IConfiguration configuration, string httpClientName)
		{
            this.WebService = WebServiceFactory.CreateWebService(serviceProvider, httpClientName);
            this.Configuration = configuration;
            this.SerializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,
            };
        }

		#endregion

		#region Method

		public async Task<OpenWeather> GetWeather(string APIKey, string longitude, string latitude, string language)
		{
			return await this.WebService.GetAsync<OpenWeather>(string.Format(WeatherZaptoConstants.UrlOWWeatherCurrentLang, latitude, longitude, APIKey, language),
                                                                        null,
                                                                        this.SerializerOptions,
                                                                        new CancellationToken()); ;
		}

		public async Task<Stream> GetWeatherImage(string code)
		{
			Stream stream = null;
			using (HttpClient client = new HttpClient())
			{
				client.Timeout = new TimeSpan(100000000); //10 sec

				using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"{string.Format(WeatherZaptoConstants.UrlOWWeatherImage, code)}"))
				{
					using (HttpResponseMessage response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, CancellationToken.None))
					{
						//GET
						if (response != null)
						{
							if (response.IsSuccessStatusCode == true)
							{
								stream = await response.Content.ReadAsStreamAsync();
							}
							else
							{
								throw new HttpRequestException();			
							}
						}
						else 
						{
							throw new HttpRequestException();
						}
					}
				}
			}
			return stream;
		}

		#endregion
	}
}
