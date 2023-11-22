using Connect.Data.Entities;
using Connect.Model;

namespace Connect.Data.Mappers
{
    internal static class ServerIotStatusMapper
    {
        public static ServerIotStatusEntity Map(ServerIotStatus model)
        {
            ServerIotStatusEntity entity = new ServerIotStatusEntity()
            {
                CreationDateTime = model.Date,
                Id = model.Id,
                ConnectionDate = model.ConnectionDate,
                IpAddress = model.IpAddress,
            };
            return entity;
        }

        public static ServerIotStatus Map(ServerIotStatusEntity entity)
        {
            ServerIotStatus model = new ServerIotStatus()
            {
                Date = entity.CreationDateTime,
                Id = entity.Id,
                ConnectionDate = entity.ConnectionDate,
                IpAddress = entity.IpAddress,
            };
            return model;
        }
    }
}
