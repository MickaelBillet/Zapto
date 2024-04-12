using Connect.Model;
using Framework.Core.Base;
using Microsoft.Extensions.DependencyInjection;

namespace Connect.Data.Supervisors
{
    public class SupervisorCacheNotification : SupervisorCache, ISupervisorNotification
    {
        #region Services
        private ISupervisorNotification Supervisor { get; }
        #endregion

        #region Constructor
        public SupervisorCacheNotification(IServiceProvider serviceProvider) : base (serviceProvider) 
        {
            this.Supervisor = serviceProvider.GetRequiredService<ISupervisorFactoryNotification>().CreateSupervisor(CacheType.None);
        }
        #endregion

        #region Methods
        public async Task Initialize()
        {
            IEnumerable<Notification> notifications = await this.Supervisor.GetNotifications();
            foreach (var item in notifications)
            {
                await this.CacheNotificationService.Set(item.Id, item);
            }
        }
        public async Task<IEnumerable<Notification>> GetNotifications()
        {
            return await this.Supervisor.GetNotifications();
        }

        public async Task<Notification> GetNotification(string id)
        {
            Notification notification = await this.CacheNotificationService.Get((arg) => arg.Id == id);
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
        public async Task<ResultCode> DeleteNotification(Notification notification)
        {
            ResultCode code = await this.Supervisor.DeleteNotification(notification);
            if (code == ResultCode.Ok) 
            {
                await this.CacheNotificationService.Delete(notification.Id);
            }
            return code;
        }
        public async Task<ResultCode> DeleteNotifications(IEnumerable<Notification> notifications)
        {
            ResultCode code = await this.Supervisor.DeleteNotifications(notifications);
            if (code == ResultCode.Ok)
            {
                foreach(Notification notification in notifications)
                {
                    await this.CacheNotificationService.Delete(notification.Id);
                }
            }
            return code;
        }
        public async Task<IEnumerable<Notification>> GetNotificationsFromRoom(string roomId)
        {
            IEnumerable<Notification> notifications = await this.CacheNotificationService.GetAll((arg) =>  arg.RoomId == roomId);
            return notifications;
        }
        public async Task<IEnumerable<Notification>> GetNotificationsFromConnectedObject(string connectedObjectId)
        {
            IEnumerable<Notification> notifications = await this.CacheNotificationService.GetAll((arg) => arg.ConnectedObjectId == connectedObjectId);
            return notifications;
        }
        #endregion
    }
}
