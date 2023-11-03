using Connect.Data.Entities;
using Connect.Model;
using Framework.Core.Base;

namespace Connect.Data.Mappers
{
    internal static class SensorMapper
    {
        public static SensorEntity Map(Sensor model)
        {
            SensorEntity entity = new SensorEntity()
            {
                CreationDateTime = model.Date,
                Id = model.Id,
                Name = model.Name,
                OffSetHumidity = model.OffSetHumidity,
                OffSetTemperature = model.OffSetTemperature,
                Channel = model.Channel,
                ConnectedObjectId = model.ConnectedObjectId,
                Humidity = model.Humidity,
                IpAddress = model.IpAddress,
                IsRunning = (model.IsRunning == RunningStatus.Healthy) ? true : false,
                LastDateTimeOn = model.LastDateTimeOn,
                LeakDetected = model.LeakDetected,
                Parameter = model.Parameter,
                Period = model.Period,
                Pressure = model.Pressure,
                Temperature = model.Temperature,
                Type = model.Type,
                RoomId = model.RoomId,
                WorkingDuration = model.WorkingDuration,
            };
            return entity;
        }

        public static Sensor Map(SensorEntity entity) 
        {
            Sensor model = new Sensor()
            {
                Date = entity.CreationDateTime,
                Id = entity.Id,
                Name = entity.Name,
                OffSetHumidity = entity.OffSetHumidity,
                OffSetTemperature = entity.OffSetTemperature,
                Channel = entity.Channel,
                ConnectedObjectId = entity.ConnectedObjectId,
                Humidity = entity.Humidity,
                IpAddress = entity.IpAddress,
                IsRunning = (entity.IsRunning == true) ? RunningStatus.Healthy : RunningStatus.UnHealthy,
                LastDateTimeOn = entity.LastDateTimeOn,
                LeakDetected = entity.LeakDetected,
                Parameter = entity.Parameter,
                Period = entity.Period,
                Pressure = entity.Pressure,
                Temperature = entity.Temperature,
                Type = entity.Type,
                RoomId = entity.RoomId,
                WorkingDuration = entity.WorkingDuration,
            };
            return model;
        }
    }
}
