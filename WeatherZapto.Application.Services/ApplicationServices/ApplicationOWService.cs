using Framework.Core.Base;
using Framework.Infrastructure.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WeatherZapto.Application.Infrastructure;
using WeatherZapto.Model;

namespace WeatherZapto.Application.Services
{
    internal class ApplicationOWService : IApplicationOWService
	{
        #region Services
        private ILocationOWService LocationOWService { get; }
        private IAirPollutionOWService AirPollutionOWService { get; }
        private IWeatherOWService WeatherOWService { get; }
        private IMemoryCache Cache { get; }
        private CacheSignal CacheSignal { get; }
        #endregion

        #region Constructor
        public ApplicationOWService(IServiceProvider serviceProvider)
		{
			this.AirPollutionOWService = serviceProvider.GetService<IAirPollutionOWService>();
            this.WeatherOWService = serviceProvider.GetService<IWeatherOWService>();
            this.LocationOWService = serviceProvider.GetService<ILocationOWService>();
            this.Cache = serviceProvider.GetService<IMemoryCache>();
			this.CacheSignal = serviceProvider.GetService<CacheSignal>();
		}
        #endregion

        #region Methods
        public async Task<ZaptoLocation> GetLocation(string APIKey, string longitude, string latitude)
        {
            ZaptoLocation zaptoLocation = null;
            Location location = (this.LocationOWService != null) ? (await this.LocationOWService.GetLocations(APIKey, longitude, latitude)).FirstOrDefault() : null;
            if (location != null)
            {
                zaptoLocation = new ZaptoLocation()
                {
                    Latitude = location.lat,
                    Longitude = location.lon,
                    Location = location.name,
                };
            }
            return zaptoLocation;
        }

        public async Task<ZaptoAirPollution> GetCurrentAirPollution(string APIKey, string locationName, string longitude, string latitude)
		{
            ZaptoAirPollution zaptoAirPollution = null;
            AirPollution airPollution = (this.AirPollutionOWService != null) ? await this.AirPollutionOWService.GetAirPollution(APIKey, longitude, latitude) : null;
            if (airPollution != null)
            {
                zaptoAirPollution = new ZaptoAirPollution()
                {
                    aqi = (airPollution?.list != null) ? airPollution?.list[0]?.main?.aqi : null,
                    co = (airPollution?.list != null) ? airPollution.list[0].components?.co : null,
                    nh3 = (airPollution?.list != null) ? airPollution.list[0].components?.nh3 : null,
                    no = (airPollution?.list != null) ? airPollution.list[0].components?.no : null,
                    no2 = (airPollution?.list != null) ? airPollution.list[0].components?.no2 : null,
                    o3 = (airPollution?.list != null) ? airPollution.list[0].components?.o3 : null,
                    pm10 = (airPollution?.list != null) ? airPollution.list[0].components?.pm10 : null,
                    pm2_5 = (airPollution?.list != null) ? airPollution.list[0].components?.pm2_5 : null,
                    so2 = (airPollution?.list != null) ? airPollution.list[0].components?.so2 : null,
                    TimeStamp = (airPollution?.list != null) ? DateTimeHelper.ParseUnixTimestamp(airPollution.list[0].dt) : default,
                    Latitude = (double.TryParse(latitude, NumberStyles.AllowDecimalPoint, new NumberFormatInfo() { NumberDecimalSeparator = "." }, out double latVal) == true) ? latVal : 0,
                    Longitude = (double.TryParse(longitude, NumberStyles.AllowDecimalPoint, new NumberFormatInfo() { NumberDecimalSeparator = "." }, out double longVal) == true) ? longVal : 0,
                    Location = locationName
                };
            }
            return zaptoAirPollution;
        }

		public async Task<ZaptoAirPollution> GetCurrentAirPollutionWithCache(string APIKey, string locationName, string longitude, string latitude)
        {
            ZaptoAirPollution zaptoAirPollution = null;
			if ((this.CacheSignal != null) && (this.Cache != null))
			{
				try
				{
					await this.CacheSignal.WaitAsync();

					if (this.Cache.TryGetValue<ZaptoAirPollution>($"AirPollution-{locationName}", out zaptoAirPollution))
					{
						Log.Information("AirPollution found");
					}
					else
					{
                        AirPollution airPollution = (this.AirPollutionOWService != null) ? await this.AirPollutionOWService.GetAirPollution(APIKey, longitude, latitude) : null;
                        if (airPollution != null)
                        {
                            zaptoAirPollution = new ZaptoAirPollution()
                            {
                                aqi = (airPollution?.list != null) ? airPollution?.list[0]?.main?.aqi : null,
                                co = (airPollution?.list != null) ? airPollution.list[0].components?.co : null,
                                nh3 = (airPollution?.list != null) ? airPollution.list[0].components?.nh3 : null,
                                no = (airPollution?.list != null) ? airPollution.list[0].components?.no : null,
                                no2 = (airPollution?.list != null) ? airPollution.list[0].components?.no2 : null,
                                o3 = (airPollution?.list != null) ? airPollution.list[0].components?.o3 : null,
                                pm10 = (airPollution?.list != null) ? airPollution.list[0].components?.pm10 : null,
                                pm2_5 = (airPollution?.list != null) ? airPollution.list[0].components?.pm2_5 : null,
                                so2 = (airPollution?.list != null) ? airPollution.list[0].components?.so2 : null,
                                TimeStamp = (airPollution?.list != null) ? DateTimeHelper.ParseUnixTimestamp(airPollution.list[0].dt) : default,
                                Latitude = (double.TryParse(latitude, NumberStyles.AllowDecimalPoint, new NumberFormatInfo() { NumberDecimalSeparator = "."}, out double latVal) == true) ? latVal : 0,
                                Longitude = (double.TryParse(longitude, NumberStyles.AllowDecimalPoint, new NumberFormatInfo() { NumberDecimalSeparator = "." }, out double longVal) == true) ? longVal : 0,
                                Location = locationName
                            };

                            var cacheEntryOptions = new MemoryCacheEntryOptions()
                                                        .SetSlidingExpiration(TimeSpan.FromSeconds(60))
                                                        .SetAbsoluteExpiration(TimeSpan.FromSeconds(900))
                                                        .SetPriority(CacheItemPriority.Normal);
                            this.Cache.Set<ZaptoAirPollution>($"AirPollution-{locationName}", zaptoAirPollution, cacheEntryOptions);
                        }
                    }
                }
				finally
				{
					this.CacheSignal.Release();
				}
			}
			return zaptoAirPollution;
        }

        public async Task<ZaptoWeather> GetCurrentWeather(string APIKey, string locationName, string longitude, string latitude, string language)
        {
            ZaptoWeather zaptoWeather = null;
            OpenWeather weather = (this.WeatherOWService != null) ? await this.WeatherOWService.GetWeather(APIKey, longitude, latitude, language) : null;
            if (weather != null)
            {
                zaptoWeather = new ZaptoWeather()
                {
                    TimeSpam = DateTimeHelper.ParseUnixTimestamp(weather.dt),
                    Latitude = (double.TryParse(latitude, NumberStyles.AllowDecimalPoint, new NumberFormatInfo() { NumberDecimalSeparator = "." }, out double latVal) == true) ? latVal : 0,
                    Longitude = (double.TryParse(longitude, NumberStyles.AllowDecimalPoint, new NumberFormatInfo() { NumberDecimalSeparator = "." }, out double longVal) == true) ? longVal : 0,
                    WindSpeed = (weather.wind != null) ? (Math.Round((weather.wind.speed * 3.6), 0)).ToString() : null,
                    WindDirection = weather.wind?.deg,
                    Temperature = (weather.main != null) ? (weather.main.temp - 273.15).ToString("0.0") : null,
                    Location = locationName,
                    WeatherText = weather.weather?.FirstOrDefault()?.description,
                    Icon = weather.weather?.FirstOrDefault()?.icon,
                };

            }
            return zaptoWeather;
        }

        public async Task<Stream> GetCurrentWeatherImage(string code)
        {
            return (this.WeatherOWService != null) ? await this.WeatherOWService.GetWeatherImage(code) : null;
        }

        public async Task<ZaptoWeather> GetCurrentWeatherWithCache(string APIKey, string locationName, string longitude, string latitude, string language)
        {
            ZaptoWeather zaptoWeather = null;
            if ((this.CacheSignal != null) && (this.Cache != null))
            {
                try
                {
                    await this.CacheSignal.WaitAsync();

                    if (this.Cache.TryGetValue<ZaptoWeather>($"OpenWeather-{locationName}", out zaptoWeather))
                    {
                        Log.Information("OpenWeather found");
                    }
                    else
                    {
                        OpenWeather weather = (this.WeatherOWService != null) ? await this.WeatherOWService.GetWeather(APIKey, longitude, latitude, language) : null;
                        if (weather != null)
                        {
                            zaptoWeather = new ZaptoWeather()
                            {
                                TimeSpam = DateTimeHelper.ParseUnixTimestamp(weather.dt),
                                Latitude = (double.TryParse(latitude, NumberStyles.AllowDecimalPoint, new NumberFormatInfo() { NumberDecimalSeparator = "." }, out double latVal) == true) ? latVal : 0,
                                Longitude = (double.TryParse(longitude, NumberStyles.AllowDecimalPoint, new NumberFormatInfo() { NumberDecimalSeparator = "." }, out double longVal) == true) ? longVal : 0,
                                Icon = weather.weather?.FirstOrDefault()?.icon,
                                WindSpeed = (weather.wind != null) ? (Math.Round((weather.wind.speed * 3.6), 0)).ToString() : null,
                                WindDirection = weather.wind?.deg,
                                Temperature = (weather.main != null) ? (weather.main.temp - 273.15).ToString("0.0") : null,
                                Location = locationName,
                                WeatherText = weather.weather?.FirstOrDefault()?.description
                            };

                            var cacheEntryOptions = new MemoryCacheEntryOptions()
                                                        .SetSlidingExpiration(TimeSpan.FromSeconds(60))
                                                        .SetAbsoluteExpiration(TimeSpan.FromSeconds(900))
                                                        .SetPriority(CacheItemPriority.Normal);
                            this.Cache.Set<ZaptoWeather>($"OpenWeather-{locationName}", zaptoWeather, cacheEntryOptions);
                        }
                    }
                }
                finally
                {
                    this.CacheSignal.Release();
                }
            }
            return zaptoWeather;
        }
        #endregion
    }
}
