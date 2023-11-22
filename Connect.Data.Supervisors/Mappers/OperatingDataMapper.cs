using Connect.Data.Entities;
using Connect.Model;

namespace Connect.Data.Mappers
{
    internal static class OperatingDataMapper
    {
        public static OperatingDataEntity Map(OperatingData model)
        {
            OperatingDataEntity entity = new OperatingDataEntity()
            {
                CreationDateTime = model.Date,
                Id = model.Id,
                ConnectedObjectId = model.ConnectedObjectId,
                PlugStatus = model.PlugStatus,
                Humidity = model.Humidity,
                PlugOrder = model.PlugOrder,
                Pressure = model.Pressure,
                RoomId = model.RoomId,
                Temperature = model.Temperature,
                WorkingDuration = model.WorkingDuration,
            };
            return entity;
        }

        public static OperatingData Map(OperatingDataEntity entity) 
        {
            OperatingData model = new OperatingData()
            {
                Date = entity.CreationDateTime,
                Id = entity.Id,
                PlugStatus = entity.PlugStatus,
                Humidity = entity.Humidity,
                PlugOrder = entity.PlugOrder,
                Pressure = entity.Pressure,
                RoomId = entity.RoomId,
                Temperature = entity.Temperature,
                WorkingDuration = entity.WorkingDuration,
                ConnectedObjectId= entity.ConnectedObjectId,
            };
            return model;
        }
    }
}
