using Connect.Data.Entities;
using Connect.Model;

namespace Connect.Data.Mappers
{
    internal static class PlugMapper
    {
        public static PlugEntity Map(Plug model)
        {
            PlugEntity entity = new PlugEntity()
            {
                CreationDateTime = model.Date,
                Id = model.Id,
                Name = model.Name,
                ConditionId = model.ConditionId,
                ConditionType = model.ConditionType,
                ConfigurationId = model.ConfigurationId,
                Status = model.Status,
                ConnectedObjectId = model.ConnectedObjectId,
                LastDateTimeOn = model.LastDateTimeOn,
                Mode = model.Mode,
                OnOff = model.OnOff,
                Order = model.Order,
                ProgramId = model.ProgramId,
                RoomId = model.RoomId,
                Type = model.Type,
                WorkingDuration = model.WorkingDuration,
                LastCommandDateTime = model.LastCommandDateTime,
                CommandReceived = model.CommandReceived,
            };
            return entity;
        }

        public static Plug Map(PlugEntity entity) 
        {
            Plug model = new Plug()
            {
                Date = entity.CreationDateTime,
                Id = entity.Id,
                Name = entity.Name,
                ConditionId = entity.ConditionId,
                ConditionType = entity.ConditionType,
                ConfigurationId = entity.ConfigurationId,
                Status = entity.Status,
                ConnectedObjectId = entity.ConnectedObjectId,
                LastDateTimeOn = entity.LastDateTimeOn,
                Mode = entity.Mode,
                OnOff = entity.OnOff,
                Order = entity.Order,
                ProgramId = entity.ProgramId,
                RoomId = entity.RoomId,
                Type = entity.Type,
                WorkingDuration = entity.WorkingDuration,
                LastCommandDateTime = entity.LastCommandDateTime,
                CommandReceived = entity.CommandReceived,
            };
            return model;
        }
    }
}
