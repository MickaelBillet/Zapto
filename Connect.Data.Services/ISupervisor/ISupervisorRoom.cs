using Connect.Model;
using Framework.Core.Base;

namespace Connect.Data
{
    public interface ISupervisorRoom
    {
        Task<ResultCode> AddRoom(Room room);
        Task<ResultCode> RoomExists(string id);
        Task<IEnumerable<Room>> GetRooms();
        Task<Room> GetRoom(string? id);
        Task<IEnumerable<Room>> GetRooms(string locationId);
        Task<Room> GetRoomFromPlugId(string plugId);
        Task<ResultCode> UpdateRoom(Room room);
    }
}
