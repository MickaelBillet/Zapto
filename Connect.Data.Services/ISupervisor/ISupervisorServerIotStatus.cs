using Connect.Model;
using Framework.Core.Base;
using System.Threading.Tasks;

namespace Connect.Data
{
    public interface ISupervisorServerIotStatus
    {
        Task<ResultCode> AddServerIotStatus(ServerIotStatus serverIotStatus);
        Task<int> CreateTable();
    }
}
