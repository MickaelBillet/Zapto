using Connect.Data.Entities;
using Connect.Model;

namespace Connect.Data.Mappers
{
    internal static class NotificationMapper
    {
        public static NotificationEntity Map(Notification model)
        {
            NotificationEntity entity = new NotificationEntity()
            {
                CreationDateTime = model.Date,
                Id = model.Id,
                ConfirmationFlag = model.ConfirmationFlag,
                ConnectedObjectId = model.ConnectedObjectId,
                IsEnabled = model.IsEnabled,
                Parameter = model.Parameter,
                RoomId = model.RoomId,
                Sign = model.Sign,
                Value = model.Value,
            };
            return entity;
        }

        public static Notification Map(NotificationEntity entity) 
        {
            Notification model = new Notification()
            {
                Date = entity.CreationDateTime,
                Id = entity.Id,
                ConfirmationFlag = entity.ConfirmationFlag,
                ConnectedObjectId = entity.ConnectedObjectId,
                IsEnabled = entity.IsEnabled,
                Parameter = entity.Parameter,
                RoomId = entity.RoomId,
                Sign = entity.Sign,
                Value = entity.Value,
            };
            return model;
        }
    }
}
