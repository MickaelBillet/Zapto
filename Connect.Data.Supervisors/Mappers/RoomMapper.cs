using Connect.Data.Entities;
using Connect.Model;

namespace Connect.Data.Mappers
{
    internal static class RoomMapper
    {
        public static RoomEntity Map(Room model)
        {
            RoomEntity entity = new RoomEntity()
            {
                CreationDateTime = model.Date,
                Id = model.Id,
                Description = model.Description,
                DeviceType = model.DeviceType,
                Humidity = model.Humidity,
                LocationId = model.LocationId,
                Name = model.Name,
                Pressure = model.Pressure,
                Temperature = model.Temperature,
                Type = model.Type,
                StatusSensors = model.StatusSensors
            };
            return entity;
        }

        public static Room Map(RoomEntity entity) 
        {
            Room model = new Room()
            {
                Date = entity.CreationDateTime,
                Id = entity.Id,
                DeviceType = entity.DeviceType,
                Humidity = entity.Humidity,
                LocationId = entity.LocationId,
                Name = entity.Name,
                Type = entity.Type,
                Temperature = entity.Temperature,
                Description = entity.Description,
                StatusSensors = entity.StatusSensors
            };
            return model;
        }
    }
}
