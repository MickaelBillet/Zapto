using Connect.Model;
using Framework.Core.Base;

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
