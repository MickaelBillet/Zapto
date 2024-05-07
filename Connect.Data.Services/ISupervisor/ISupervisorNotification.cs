using Connect.Model;
using Framework.Core.Base;

namespace Connect.Data
{
    public interface ISupervisorNotification : ISupervisor
    {
        Task<Notification> GetNotification(string id);
        Task<ResultCode> AddNotification(Notification notification);
        Task<ResultCode> DeleteNotification(Notification notification);
        Task<ResultCode> UpdateNotification(string id, Notification notification);
        Task UpdateNotifications(IEnumerable<Notification> notifications);
        Task<ResultCode> DeleteNotifications(IEnumerable<Notification> notifications);
        Task<IEnumerable<Notification>> GetNotificationsFromRoom(string roomId);
        Task<IEnumerable<Notification>> GetNotificationsFromConnectedObject(string connectedObjectId);
        Task<IEnumerable<Notification>> GetNotifications();
    }
}
