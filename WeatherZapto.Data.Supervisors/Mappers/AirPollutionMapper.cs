using WeatherZapto.Data.Entities;
using WeatherZapto.Model;

namespace WeatherZapto.Data.Mappers
{
    internal static class AirPollutionMapper
    {
        public static AirPollutionEntity Map(ZaptoAirPollution model)
        {
            return new AirPollutionEntity()
            {
                TimeStamp = model.TimeStamp,
                CreationDateTime = model.Date,
                aqi = model.aqi,
                co = model.co,
                nh3 = model.nh3,
                no = model.no,
                no2 = model.no2,
                o3 = model.o3,
                pm10 = model.pm10,
                pm2_5 = model.pm2_5,
                so2 = model.so2,
                Id = model.Id,
                Latitude = model.Latitude,
                Location = model.Location,
                Longitude = model.Longitude,
            };
        }

        public static ZaptoAirPollution Map(AirPollutionEntity entity)
        {
            return new ZaptoAirPollution()
            {
                aqi = entity.aqi,
                co = entity.co,
                nh3 = entity.nh3,
                no = entity.no,
                no2 = entity.no2,
                o3 = entity.o3,
                pm10 = entity.pm10,
                pm2_5 = entity.pm2_5,
                so2 = entity.so2,
                Latitude = entity.Latitude,
                Longitude = entity.Longitude,
                Location = entity.Location,
                Date = entity.CreationDateTime,
                TimeStamp = entity.TimeStamp,
                Id = entity.Id,
            };
        }
    }
}
