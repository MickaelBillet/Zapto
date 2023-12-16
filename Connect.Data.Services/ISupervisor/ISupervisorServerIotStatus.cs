using Connect.Model;
using Framework.Core.Base;

namespace Connect.Data
{
    public interface ISupervisorServerIotStatus
    {
        Task<ResultCode> AddServerIotStatus(ServerIotStatus serverIotStatus);
        Task<ResultCode> CreateTable();
    }
}
