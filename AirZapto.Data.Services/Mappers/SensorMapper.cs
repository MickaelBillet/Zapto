using AirZapto.Data.Entities;
using AirZapto.Model;
using Framework.Core.Base;
using System.Text.Json;

namespace AirZapto.Data.Mappers
{
	internal static class SensorMapper
	{
		public static SensorEntity Map(Sensor sensor)
		{
			SensorEntity entity = new SensorEntity()
			{
				IdSocket = sensor.IdSocket,
				Name = sensor.Name ?? string.Empty,
				Channel = sensor.Channel,
				CO2 = sensor.CO2,
				Temperature = sensor.Temperature,
				Id = sensor.Id,
				Pressure = sensor.Pressure,
				Humidity = sensor.Humidity,
				Description = sensor.Description ?? string.Empty,
				Type = sensor.Type,
				Period = sensor.Period ?? string.Empty,
				ThresholdCO2 = JsonSerializer.Serialize<int[]>(sensor.ThresholdCO2),
				Mode = sensor.Mode,
				CreationDateTime = sensor.Date,
				IsRunning= sensor.IsRunning,
			};

			return entity;
		}

		public static Sensor Map(SensorEntity entity)
		{
			Sensor sensor = new Sensor()
			{
				IdSocket = entity.IdSocket,
				Name = entity.Name,
				Channel = entity.Channel,
				CO2 = entity.CO2,
				Temperature = entity.Temperature,
				Id = entity.Id,
				Pressure = entity.Pressure,
				Humidity = entity.Humidity,
				Description = entity.Description,
				Type = entity.Type,
				Period = entity.Period,
				ThresholdCO2 = JsonSerializer.Deserialize<int[]>(entity.ThresholdCO2) ?? new int[]{ 800, 1100 },
				Mode = entity.Mode,
				Date = entity.CreationDateTime,
				IsRunning = entity.IsRunning,
			};

			return sensor;
		}
	}
}
