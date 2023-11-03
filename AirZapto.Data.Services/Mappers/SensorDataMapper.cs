using AirZapto.Data.Entities;
using AirZapto.Model;
using Framework.Core.Base;

namespace AirZapto.Data.Mappers
{
	internal static class SensorDataMapper
	{
		public static SensorDataEntity Map(AirZaptoData data)
		{
			SensorDataEntity entity = new SensorDataEntity()
			{
				Id = data.Id,
				Temperature = data.Temperature,
				CO2 = data.CO2,
				CreationDateTime = data.Date,
				SensorId = data.SensorId,					
			};

			return entity;
		}

		public static AirZaptoData Map(SensorDataEntity entity)
		{
			AirZaptoData data = new AirZaptoData()
			{
				Id = entity.Id,
				Temperature = entity.Temperature,
				CO2 = entity.CO2,
				SensorId = entity.SensorId,
				Date = entity.CreationDateTime,				
			};

			return data;
		}
	}
}
