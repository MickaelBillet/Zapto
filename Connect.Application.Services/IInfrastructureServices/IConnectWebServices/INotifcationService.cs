using Connect.Model;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace Connect.Application.Infrastructure
{
    public interface INotificationService
    {
        Task<bool?> DeleteNotificationAsync(Notification notification, CancellationToken token = default);

        Task<bool?> AddNotificationAsync(Notification notification, CancellationToken token = default);

        Task<bool?> UpdateNotificationAsync(Notification notification, CancellationToken token = default);

        Task<ObservableCollection<Notification>?> GetFromRoomAsync(string roomId, CancellationToken token = default);

        Task<ObservableCollection<Notification>?> GetFromConnectedObjectAsync(string connectedObjectId, CancellationToken token = default);

        Task<bool?> DeleteFromRoomAsync(string roomId, CancellationToken token = default);

        Task<bool?> DeleteFromConnectedObjectAsync(string connectedObjectid, CancellationToken token = default);
    }
}
