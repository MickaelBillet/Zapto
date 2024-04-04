using Connect.Model;
using Framework.Core.Base;
using Microsoft.Extensions.DependencyInjection;

namespace Connect.Data.Supervisors
{
    public class SupervisorCacheNotification : SupervisorCache
    {
        #region Services
        private ISupervisorNotification Supervisor { get; }
        #endregion

        #region Constructor
        public SupervisorCacheNotification(IServiceProvider serviceProvider) : base (serviceProvider) 
        {
            this.Supervisor = serviceProvider.GetRequiredService<ISupervisorNotification>();
        }
        #endregion

        #region Methods
        public async Task<Notification> GetNotification(string id)
        {
            Notification notification = await this.CacheNotificationService.Get((arg) => arg.Id == id);
            if (notification == null) 
            {
                notification = await this.Supervisor.GetNotification(id);
            }
            return notification;
        }
        public async Task<ResultCode> AddNotification(Notification notification)
        {
            ResultCode code = await this.Supervisor.AddNotification(notification);
            if (code == ResultCode.Ok)
            {
                await this.CacheNotificationService.Set(notification.Id, notification);
            }
            return code;
        }
        public async Task<ResultCode> UpdateNotification(string id, Notification notification)
        {
            ResultCode code = await this.Supervisor.UpdateNotification(id, notification);
            if (code == ResultCode.Ok)
            {
                await this.CacheNotificationService.Set(id, notification);
            }
            return code;
        }
        public async Task UpdateNotifications(IEnumerable<Notification> notifications)
        {
            await this.Supervisor.UpdateNotifications(notifications);
            foreach (Notification notification in notifications)
            {
                await this.CacheNotificationService.Delete(notification.Id);
            }
        }
        public async Task<IEnumerable<Notification>> GetNotificationsFromRoom(string roomId)
        {
            IEnumerable<Notification> notifications = await this.CacheNotificationService.GetAll((arg) =>  arg.RoomId == roomId);
            if (notifications == null)
            {
                notifications  = await this.Supervisor.GetNotificationsFromRoom(roomId);
            }
            return notifications;
        }
        public async Task<IEnumerable<Notification>> GetNotificationsFromConnectedObject(string connectedObjectId)
        {
            IEnumerable<Notification> notifications = await this.CacheNotificationService.GetAll((arg) => arg.ConnectedObjectId == connectedObjectId);
            if (notifications == null)
            {
                notifications = await this.Supervisor.GetNotificationsFromConnectedObject(connectedObjectId);
            }
            return notifications;
        }
        #endregion
    }
}
