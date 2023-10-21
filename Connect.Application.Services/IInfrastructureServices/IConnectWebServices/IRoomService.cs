using Connect.Model;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Connect.Application.Infrastructure
{
    public interface IRoomService
    {
        IObservable<Room> GetRoom(string roomId, Boolean forceRefresh = true);
        Task<IEnumerable<Room>?> GetRooms(string? locationId, CancellationToken token = default);
    }
}
