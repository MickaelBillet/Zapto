using Connect.Model;
using Framework.Core.Base;

namespace Connect.Data
{
    public interface ISupervisorCacheRoom : ISupervisorCache
    {
        public Task<IEnumerable<Room>> GetRooms();
        public Task<IEnumerable<Room>> GetRooms(string locationId);
        public Task<Room> GetRoomFromPlugId(string plugId);
        public Task<ResultCode> UpdateRoom(Room room);
        public Task<ResultCode> AddRoom(Room room);
        public Task<Room> GetRoom(string? id);
    }
}
