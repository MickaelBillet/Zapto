using Connect.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Connect.Application
{
	public interface IApplicationConnectedObjectServices
	{
		Task SendDataToClientAsync(string locationId, ConnectedObject @object);
		Task NotifiyConnectedObjectCondition(IEnumerable<Notification> notifications, Room room, ConnectedObject connectedObject);
    }
}
