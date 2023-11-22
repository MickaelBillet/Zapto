using Connect.Data.Entities;
using Connect.Data.Mappers;
using Connect.Data.Services.Repositories;
using Connect.Model;
using Framework.Core.Base;
using Framework.Data.Abstractions;

namespace Connect.Data.Supervisors
{
    public sealed class SupervisorNotification : ISupervisorNotification
    {
        private readonly Lazy<IRepository<NotificationEntity>> _lazyNotificationRepository;

        #region Properties
        private IRepository<NotificationEntity> NotificationRepository => _lazyNotificationRepository.Value;
        #endregion

        #region Constructor
        public SupervisorNotification(IDalSession session, IRepositoryFactory repositoryFactory)
        {
            _lazyNotificationRepository = repositoryFactory.CreateRepository<NotificationEntity>(session);
        }
        #endregion

        #region Methods
        public async Task<ResultCode> NotificationExists(string id)
        {
            return (await this.NotificationRepository.GetAsync(id) != null) ? ResultCode.Ok : ResultCode.ItemNotFound;
        }

        public async Task<IEnumerable<Notification>> GetNotifications()
        {
            IEnumerable<NotificationEntity> entities = await this.NotificationRepository.GetCollectionAsync();
            return entities.Select(item => NotificationMapper.Map(item));
        }

        public async Task<Notification> GetNotification(string id)
        {
            return NotificationMapper.Map(await this.NotificationRepository.GetAsync(id));
        }

        public async Task<ResultCode> AddNotification(Notification notification)
        {
            notification.Id = string.IsNullOrEmpty(notification.Id) ? Guid.NewGuid().ToString() : notification.Id;
            int res = await this.NotificationRepository.InsertAsync(NotificationMapper.Map(notification));
            ResultCode result = (res > 0) ? ResultCode.Ok : ResultCode.CouldNotCreateItem;
            return result;
        }

        public async Task<ResultCode> DeleteNotification(Notification notification)
        {
            return (await this.NotificationRepository.DeleteAsync(NotificationMapper.Map(notification)) > 0) ? ResultCode.Ok : ResultCode.CouldNotDeleteItem;
        }

        public async Task<ResultCode> DeleteNotifications(IEnumerable<Notification> notifications)
        {
            if (notifications.Count() == 0)
            {
                return ResultCode.Ok;
            }
            else
            {
                int count = notifications.Count();
                foreach (Notification notification in notifications)
                {
                    if (await this.NotificationRepository.DeleteAsync(NotificationMapper.Map(notification)) > 0)
                    {
                        count--;
                    }
                }

                if (count == 0)
                {
                    return ResultCode.Ok;
                }
            }

            return ResultCode.CouldNotDeleteItem;
        }

        public async Task<ResultCode> UpdateNotification(string id, Notification notification)
        {
            ResultCode result = await this.NotificationExists(id);

            if (result == ResultCode.Ok)
            {
                result = (await this.NotificationRepository.UpdateAsync(NotificationMapper.Map(notification)) > 0) ? ResultCode.Ok : ResultCode.CouldNotUpdateItem;
            }

            return (result);
        }

        public async Task UpdateNotifications(IEnumerable<Notification> notifications)
        {
            if (notifications?.Count() > 0)
            {
                foreach (Notification notification in notifications)
                {
                    ResultCode result = await this.NotificationExists(notification.Id);

                    if (result == ResultCode.Ok)
                    {
                        result = (await this.NotificationRepository.UpdateAsync(NotificationMapper.Map(notification)) > 0) ? ResultCode.Ok : ResultCode.CouldNotUpdateItem;
                    }
                    else
                    {
                        result = ResultCode.ItemNotFound;
                    }
                }
            }
        }

        public async Task<IEnumerable<Notification>> GetNotificationsFromRoom(string roomId)
        {
            //Read the notifications of the room
            IEnumerable<NotificationEntity> entities = await this.NotificationRepository.GetCollectionAsync((arg) => arg.RoomId == roomId);
            return entities.Select(item => NotificationMapper.Map(item));
        }

        public async Task<IEnumerable<Notification>> GetNotificationsFromConnectedObject(string connectedObjectId)
        {
            //Read the notifications of the room
            IEnumerable<NotificationEntity> entities = await this.NotificationRepository.GetCollectionAsync((arg) => arg.ConnectedObjectId == connectedObjectId);
            return entities.Select(item => NotificationMapper.Map(item));
        }

        #endregion
    }
}
