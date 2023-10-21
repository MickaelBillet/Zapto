using Connect.Model;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Connect.Application
{
    public interface IApplicationOperationDataService
    {
        Task<IEnumerable<OperatingData>> GetRoomOperatingDataOfDay(string roomId, DateTime? day, CancellationToken token = default);
        Task<DateTime?> GetRoomMaxDate(string roomId, CancellationToken token = default);
        Task<DateTime?> GetRoomMinDate(string roomId, CancellationToken token = default);
    }
}
