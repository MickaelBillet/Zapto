using Connect.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Connect.Application
{
	public interface IApplicationRoomServices
	{
		IObservable<Room>? GetRoom(string roomId);
		Task SendDataToClient(string locationId, Room room);
		Task<IEnumerable<Room>?> GetRooms(string? locationId);
		Task NotifiyRoomCondition(string locationId, IEnumerable<Notification> notifications, Room room);
    }
}
