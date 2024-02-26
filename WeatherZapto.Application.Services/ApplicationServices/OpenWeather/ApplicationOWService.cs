using Framework.Core.Base;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using WeatherZapto.Application.Infrastructure;
using WeatherZapto.Data;
using WeatherZapto.Model;

namespace WeatherZapto.Application.Services
{
    internal class ApplicationOWService : IApplicationOWService
    {
        #region Services
        private ILocationOWService LocationOWService { get; }
        private IAirPollutionOWService AirPollutionOWService { get; }
        private IWeatherOWService WeatherOWService { get; }
        protected IServiceScopeFactory ServiceScopeFactory { get; set; }
        #endregion

        #region Constructor
        public ApplicationOWService(IServiceProvider serviceProvider)
        {
            this.AirPollutionOWService = serviceProvider.GetService<IAirPollutionOWService>();
            this.WeatherOWService = serviceProvider.GetService<IWeatherOWService>();
            this.LocationOWService = serviceProvider.GetService<ILocationOWService>();
            this.ServiceScopeFactory = serviceProvider.GetService<IServiceScopeFactory>();
        }
        #endregion

        #region Methods
        public async Task<IEnumerable<ZaptoLocation>> GetLocationFromCity(string APIKey, string city, string stateCode, string countryCode)
        {
            IEnumerable<ZaptoLocation> zaptoLocations = null;
            IEnumerable<Location> locations = LocationOWService != null ? await LocationOWService.GetLocationsFromCity(APIKey, city, stateCode, countryCode) : null;
            if (locations != null)
            {
                using (IServiceScope scope = this.ServiceScopeFactory.CreateScope())
                {
                    await scope.ServiceProvider.GetRequiredService<ISupervisorCall>().AddCallOpenWeather();

                    zaptoLocations = locations.Select((location) =>
                    {
                        return new ZaptoLocation()
                        {
                            Latitude = location.lat,
                            Longitude = location.lon,
                            Location = location.name,
                            Country = location.country,
                            State = location.state,
                        };
                    });
                };
            }
            return zaptoLocations;
        }

        public async Task<ZaptoLocation> GetLocationFromZipCode(string APIKey, string zipCode, string countryCode)
        {
            ZaptoLocation zaptoLocation = null;
            Location location = LocationOWService != null ? await LocationOWService.GetLocationFromZipCode(APIKey, zipCode, countryCode) : null;
            if (location != null)
            {
                using (IServiceScope scope = this.ServiceScopeFactory.CreateScope())
                {
                    await scope.ServiceProvider.GetRequiredService<ISupervisorCall>().AddCallOpenWeather(); return new ZaptoLocation()
                    {
                        Latitude = location.lat,
                        Longitude = location.lon,
                        Location = location.name,
                        Country = location.country,
                        Zip = location.zip,
                    };
                };
            }
            return zaptoLocation;
        }

        public async Task<ZaptoLocation> GetReverseLocation(string APIKey, string longitude, string latitude)
        {
            ZaptoLocation zaptoLocation = null;
            Location location = LocationOWService != null ? (await LocationOWService.GetReverseLocations(APIKey, longitude, latitude)).FirstOrDefault() : null;
            if (location != null)
            {
                using (IServiceScope scope = this.ServiceScopeFactory.CreateScope())
                {
                    await scope.ServiceProvider.GetRequiredService<ISupervisorCall>().AddCallOpenWeather(); zaptoLocation = new ZaptoLocation()
                    {
                        Latitude = location.lat,
                        Longitude = location.lon,
                        Location = location.name,
                    };
                }
            }
            return zaptoLocation;
        }

        public async Task<ZaptoAirPollution> GetCurrentAirPollution(string APIKey, string locationName, string longitude, string latitude)
        {
            ZaptoAirPollution zaptoAirPollution = null;
            AirPollution airPollution = AirPollutionOWService != null ? await AirPollutionOWService.GetAirPollution(APIKey, longitude, latitude) : null;
            if (airPollution != null)
            {
                using (IServiceScope scope = this.ServiceScopeFactory.CreateScope())
                {
                    await scope.ServiceProvider.GetRequiredService<ISupervisorCall>().AddCallOpenWeather(); 
                    
                    zaptoAirPollution = new ZaptoAirPollution()
                    {
                        aqi = airPollution?.list != null ? airPollution?.list[0]?.main?.aqi : null,
                        co = airPollution?.list != null ? airPollution.list[0].components?.co : null,
                        nh3 = airPollution?.list != null ? airPollution.list[0].components?.nh3 : null,
                        no = airPollution?.list != null ? airPollution.list[0].components?.no : null,
                        no2 = airPollution?.list != null ? airPollution.list[0].components?.no2 : null,
                        o3 = airPollution?.list != null ? airPollution.list[0].components?.o3 : null,
                        pm10 = airPollution?.list != null ? airPollution.list[0].components?.pm10 : null,
                        pm2_5 = airPollution?.list != null ? airPollution.list[0].components?.pm2_5 : null,
                        so2 = airPollution?.list != null ? airPollution.list[0].components?.so2 : null,
                        TimeStamp = airPollution?.list != null ? DateTimeHelper.ParseUnixTimestamp(airPollution.list[0].dt) : default,
                        Latitude = double.TryParse(latitude, NumberStyles.AllowDecimalPoint, new NumberFormatInfo() { NumberDecimalSeparator = "." }, out double latVal) == true ? latVal : 0,
                        Longitude = double.TryParse(longitude, NumberStyles.AllowDecimalPoint, new NumberFormatInfo() { NumberDecimalSeparator = "." }, out double longVal) == true ? longVal : 0,
                        Location = locationName
                    };
                }
            }
            return zaptoAirPollution;
        }

        public async Task<ZaptoWeather> GetCurrentWeather(string APIKey, string locationName, string longitude, string latitude, string language)
        {
            ZaptoWeather zaptoWeather = null;
            OpenWeather weather = WeatherOWService != null ? await WeatherOWService.GetWeather(APIKey, longitude, latitude, language) : null;
            if (weather != null)
            {
                using (IServiceScope scope = this.ServiceScopeFactory.CreateScope())
                {
                    await scope.ServiceProvider.GetRequiredService<ISupervisorCall>().AddCallOpenWeather();                 

                    zaptoWeather = new ZaptoWeather()
                    {
                        TimeSpam = DateTimeHelper.ParseUnixTimestamp(weather.dt),
                        Latitude = double.TryParse(latitude, NumberStyles.AllowDecimalPoint, new NumberFormatInfo() { NumberDecimalSeparator = "." }, out double latVal) == true ? latVal : 0,
                        Longitude = double.TryParse(longitude, NumberStyles.AllowDecimalPoint, new NumberFormatInfo() { NumberDecimalSeparator = "." }, out double longVal) == true ? longVal : 0,
                        WindSpeed = weather.wind != null ? Math.Round(weather.wind.speed * 3.6, 0).ToString() : null,
                        WindDirection = weather.wind?.deg,
                        Temperature = weather.main != null ? (weather.main.temp - 273.15).ToString("0.0") : null,
                        FeelsLike = weather.main != null ? (weather.main.feels_like - 273.15).ToString("0.0") : null,
                        Pressure = weather.main?.pressure,
                        Location = locationName,
                        WeatherText = weather.weather?.FirstOrDefault()?.description,
                        Icon = weather.weather?.FirstOrDefault()?.icon,
                    };
                }

            }
            return zaptoWeather;
        }

        public async Task<Stream> GetCurrentWeatherImage(string code)
        {
            return WeatherOWService != null ? await WeatherOWService.GetWeatherImage(code) : null;
        }
        #endregion
    }
}
