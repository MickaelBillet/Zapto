using Connect.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Connect.Application
{
	public interface IApplicationRoomServices
	{
		IObservable<Room>? GetRoom(string roomId);
		Task SendDataToClientAsync(string locationId, Room room);
		Task<IEnumerable<Room>?> GetRoomsAsync(string? locationId);
		Task NotifiyRoomCondition(string locationId, IEnumerable<Notification> notifications, Room room);
    }
}
