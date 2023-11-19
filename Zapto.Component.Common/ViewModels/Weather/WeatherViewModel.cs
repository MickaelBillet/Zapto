using Blazored.LocalStorage;
using Framework.Core.Domain;
using Microsoft.Extensions.DependencyInjection;
using WeatherZapto.Application;
using WeatherZapto.Model;
using Zapto.Component.Common.Models;

namespace Zapto.Component.Common.ViewModels
{
    public interface IWeatherViewModel : IBaseViewModel
    {
		Task<WeatherModel?> GetWeatherModel();
		Task<WeatherModel?> GetWeatherModel(string latitude, string longitude);
    }

	public class WeatherViewModel : BaseViewModel, IWeatherViewModel
    {
		#region Properties
		private IApplicationWeatherService ApplicationWeatherService { get; }
		private IApplicationOWService ApplicationOWService { get; }
        private ILocalStorageService LocalStorageService { get; }
        #endregion

        #region Constructor
        public WeatherViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
		{
			this.ApplicationWeatherService = serviceProvider.GetRequiredService<IApplicationWeatherService>();
			this.ApplicationOWService = serviceProvider.GetRequiredService<IApplicationOWService>();
            this.LocalStorageService = serviceProvider.GetRequiredService<ILocalStorageService>();
        }
        #endregion

        #region Methods
        public override async Task InitializeAsync(string? parameter)
		{
			await base.InitializeAsync(parameter);
		}
		public override void Dispose()
		{
			base.Dispose();
		}
		public async Task<WeatherModel?> GetWeatherModel()
		{
			WeatherModel model = new WeatherModel();

			ZaptoUser user = await this.AuthenticationService.GetAuthenticatedUser();
			if (user != null)
			{
				string culture = await this.LocalStorageService.GetItemAsync<string>("culture");
				if (string.IsNullOrEmpty(culture) == false)
				{
					ZaptoWeather? weather = await this.ApplicationWeatherService.GetCurrentWeather(user.LocationName, user.LocationLongitude, user.LocationLatitude, culture);
					if (weather != null)
					{
						model.Temperature = weather.Temperature;
						model.WeatherText = weather.WeatherText;
						model.WindSpeed = weather.WindSpeed;
						model.WindDirection = weather.WindDirection;
						model.Location = weather.Location;

						using (Stream stream = await this.ApplicationOWService.GetCurrentWeatherImage(weather.Icon))
						{
							byte[]? byteArray = (stream as MemoryStream)?.ToArray();
							if (byteArray != null)
							{
								string? b64String = Convert.ToBase64String(byteArray);
								model.Image = "data:image/png;base64," + b64String;
							}
						}
					}
				}
			}

            return model;
		}

        public async Task<WeatherModel?> GetWeatherModel(string latitude, string longitude)
        {
			WeatherModel? model = null;

            string culture = await this.LocalStorageService.GetItemAsync<string>("culture");
            if (string.IsNullOrEmpty(culture) == false)
            {
                ZaptoWeather? weather = await this.ApplicationWeatherService.GetCurrentWeather(longitude, latitude, culture);
                if (weather != null)
                {
					model = new WeatherModel();
                    model.Temperature = weather.Temperature;
                    model.WeatherText = weather.WeatherText;
                    model.WindSpeed = weather.WindSpeed;
                    model.WindDirection = weather.WindDirection;
                    model.Location = weather.Location;

                    using (Stream stream = await this.ApplicationOWService.GetCurrentWeatherImage(weather.Icon))
                    {
                        byte[]? byteArray = (stream as MemoryStream)?.ToArray();
                        if (byteArray != null)
                        {
                            string? b64String = Convert.ToBase64String(byteArray);
                            model.Image = "data:image/png;base64," + b64String;
                        }
                    }
                }
            }

            return model;
        }
        #endregion
    }
}
