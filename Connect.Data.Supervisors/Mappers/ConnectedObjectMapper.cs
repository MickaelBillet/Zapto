using Connect.Data.Entities;
using Connect.Model;

namespace Connect.Data.Mappers
{
    internal static class ConnectedObjectMapper
    {
        public static ConnectedObjectEntity Map(ConnectedObject model)
        {
            ConnectedObjectEntity entity = new ConnectedObjectEntity()
            {
                CreationDateTime = model.Date,
                Id = model.Id,
                DeviceType = model.DeviceType,  
                Name = model.Name,
                RoomId = model.RoomId,
            };
            return entity;
        }

        public static ConnectedObject Map(ConnectedObjectEntity entity) 
        {
            ConnectedObject model = new ConnectedObject()
            {
                Date = entity.CreationDateTime,
                Id = entity.Id,
                DeviceType = entity.DeviceType,
                Name= entity.Name,
                RoomId= entity.RoomId,
            };
            return model;
        }
    }
}
