using Connect.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Connect.Application
{
    public interface IApplicationNotificationServices
	{
		Task<ICollection<Notification>?> GetNotifications(INotificationProvider provider);
		Task<bool?> DeleteNotification(INotificationProvider provider, Notification notification);
		Task<bool?> DeleteNotifications(INotificationProvider provider);
		Task<bool?> AddUpdateNotification(INotificationProvider provider, Notification notification);
	}
}
