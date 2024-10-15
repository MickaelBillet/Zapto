using Connect.Model;
using Framework.Core.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Connect.Data
{
    public interface ISupervisorOperatingData
    {
        Task<ResultCode> AddOperatingData(OperatingData data);
        Task<ResultCode> DeleteOperationData(DateTime dateTime, string roomId);
        Task<IEnumerable<OperatingData>> GetRoomOperatingDataOfDay(string roomId, DateTime day);
        Task<DateTime?> GetRoomMaxDate(string roomId);
        Task<DateTime?> GetRoomMinDate(string roomId);
    }
}
