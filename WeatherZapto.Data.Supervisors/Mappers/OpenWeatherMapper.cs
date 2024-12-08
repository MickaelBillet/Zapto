using IdentityModel;
using WeatherZapto.Data.Entities;
using WeatherZapto.Model;

namespace WeatherZapto.Data.Mappers
{
    internal static class OpenWeatherMapper
    {
        public static WeatherEntity Map(ZaptoWeather model)
        {
            return new WeatherEntity()
            {
                CreationDateTime = model.Date,
                Icon = model.Icon,
                Id = model.Id,
                WindSpeed = model.WindSpeed,
                WeatherText = model.WeatherText,
                WindDirection = model.WindDirection,
                Temperature = double.Parse(model.Temperature),
                Latitude = model.Latitude,
                Location = model.Location,
                Longitude = model.Longitude,
                TimeStamp = model.TimeSpam.Value,
            };
        }

        public static ZaptoWeather Map(WeatherEntity entity)
        { 
            return new ZaptoWeather()
            {
                Icon = entity.Icon,
                Id = entity.Id,
                WeatherText = entity.WeatherText,
                Temperature= entity.Temperature.ToString(),
                WindDirection = entity.WindDirection,
                WindSpeed = entity.WindSpeed,
                Latitude = entity.Latitude,
                Longitude = entity.Longitude,
                Location = entity.Location,
                Date = entity.CreationDateTime,
                TimeSpam = entity.TimeStamp,
            }; 
        }
    }
}
