using Connect.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Connect.Application
{
    public interface IApplicationNotificationServices
	{
		Task<ICollection<Notification>?> GetNotificationsAsync(INotificationProvider provider);
		Task<bool?> DeleteNotificationAsync(INotificationProvider provider, Notification notification);
		Task<bool?> DeleteNotificationsAsync(INotificationProvider provider);
		Task<bool?> AddUpdateNotificationAsync(INotificationProvider provider, Notification notification);
	}
}
