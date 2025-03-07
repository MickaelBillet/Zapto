﻿using Connect.Model;
using Framework.Core.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Connect.Data
{
    public interface ISupervisorRoom : ISupervisor
    {
        Task<ResultCode> AddRoom(Room room);
        Task<IEnumerable<Room>> GetRooms();
        Task<Room> GetRoom(string? id);
        Task<IEnumerable<Room>> GetRooms(string locationId);
        Task<Room> GetRoomFromPlugId(string plugId);
        Task<ResultCode> UpdateRoom(Room room);
    }
}
