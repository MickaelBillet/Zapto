using Framework.Core.Base;
using Framework.Core.Domain;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Globalization;
using WeatherZapto.Application;
using WeatherZapto.Model;
using Zapto.Component.Common.Models;
using Zapto.Component.Common.Services;

namespace Zapto.Component.Common.ViewModels
{
    public interface IWeatherViewModel : IBaseViewModel
    {
		Task<(WeatherModel? model, bool hasError)> GetWeatherModel();
		Task<WeatherModel?> GetWeatherModel(LocationModel location);
    }

	public class WeatherViewModel : BaseViewModel, IWeatherViewModel
    {
		#region Properties
		private IApplicationWeatherService ApplicationWeatherService { get; }
		private IApplicationOWService ApplicationOWService { get; }
        private IZaptoStorageService LocalStorageService { get; }
        #endregion

        #region Constructor
        public WeatherViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
		{
			this.ApplicationWeatherService = serviceProvider.GetRequiredService<IApplicationWeatherService>();
			this.ApplicationOWService = serviceProvider.GetRequiredService<IApplicationOWService>();
            this.LocalStorageService = serviceProvider.GetRequiredService<IZaptoStorageService>();
        }
        #endregion

        #region Methods
        public async Task<(WeatherModel? model, bool hasError)> GetWeatherModel()
		{
			WeatherModel? model = null;
            bool hasError = false;

			try
			{
				ZaptoUser? user = await this.AuthenticationService.GetAuthenticatedUser();
				if (user != null)
				{
					string? culture = await this.LocalStorageService.GetItemAsync<string>("blazorCulture");
                    Log.Debug($"Culture : {culture}");

					if (string.IsNullOrEmpty(culture) == false)
					{
                        this.IsLoading = true;

						ZaptoWeather? weather = await this.ApplicationWeatherService.GetCurrentWeather(user.LocationName, user.LocationLongitude, user.LocationLatitude, culture);
						if (weather != null)
						{
							model = new WeatherModel()
							{
								Temperature = weather.Temperature,
                                TemperatureMax = this.GetTemperatureMax(weather),
                                TemperatureMin = this.GetTemperatureMin(weather),
                                FeelsLike = weather.FeelsLike,
                                Pressure = weather.Pressure,
								WeatherText = weather.WeatherText,
								WindSpeed = weather.WindSpeed,
								WindDirection = weather.WindDirection,
								Location = weather.Location,
							};

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
			}
            catch (Exception ex)
            {
                Log.Debug($"{ClassHelper.GetCallerClassAndMethodName()} - {ex.ToString()}");
                hasError = true;
                this.NavigationService.ShowMessage("Weather Service Exception", ZaptoSeverity.Error);
            }
            finally
            {
                this.IsLoading = false;
            }
            return (model, hasError);
		}

        public async Task<WeatherModel?> GetWeatherModel(LocationModel location)
        {
			WeatherModel? model = null;
            try
            {
                if ((location != null) && (location.Latitude != null) && (location.Longitude != null))
                {
                    string? culture = await this.LocalStorageService.GetItemAsync<string>("blazorCulture");
                    Log.Debug($"Culture : {culture}");

                    if (string.IsNullOrEmpty(culture) == false)
                    {
                        this.IsLoading = true;

                        ZaptoWeather? weather = await this.ApplicationWeatherService.GetCurrentWeather(location.Location, location.Longitude.ToString(), location.Latitude.ToString(), culture);
                        if (weather != null)
                        {
                            model = new WeatherModel()
                            {
                                Temperature = weather.Temperature,
                                FeelsLike = weather.FeelsLike,
                                Pressure = weather.Pressure,
                                WeatherText = weather.WeatherText,
                                WindSpeed = weather.WindSpeed,
                                WindDirection = weather.WindDirection,
                                Location = weather.Location,
                            };

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
            }
            catch (Exception ex)
            {
                Log.Debug($"{ClassHelper.GetCallerClassAndMethodName()} - {ex.ToString()}");
                this.NavigationService.ShowMessage("Weather Service Exception", ZaptoSeverity.Error);
            }
            finally
            {
                this.IsLoading = false;
            }
            return model;
        }

        private string GetTemperatureMax(ZaptoWeather weather)
        {
            string max = string.Empty;
            if ((string.IsNullOrEmpty(weather.TemperatureMax) == false)
                && (string.IsNullOrEmpty(weather.Temperature) == false))
            {
                if (double.Parse(weather.Temperature.Replace('.', ','), NumberStyles.Number) > double.Parse(weather.TemperatureMax.Replace('.', ','), NumberStyles.Number))
                { 
                    max = weather.Temperature;
                }
                else
                {
                    max = weather.TemperatureMax;
                }
            }
            return max;
        }

        private string GetTemperatureMin(ZaptoWeather weather)
        {
            string min = string.Empty;
            if ((string.IsNullOrEmpty(weather.TemperatureMin) == false)
                && (string.IsNullOrEmpty(weather.Temperature) == false))
            {
                if (double.Parse(weather.Temperature.Replace('.', ','), NumberStyles.Number) < double.Parse(weather.TemperatureMin.Replace('.', ','), NumberStyles.Number))
                {
                    min = weather.Temperature;
                }
                else
                {
                    min = weather.TemperatureMin;
                }
            }
            return min;
        }
        #endregion
    }
}
