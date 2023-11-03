using Framework.Core.Base;
using System;
using System.Threading.Tasks;

namespace Connect.Data
{
    public interface ISupervisorVersion
    {
        Task<Version> GetVersion();
        Task<ResultCode> AddVersion();
        Task<ResultCode> UpdateVersion(int major, int minor, int build);
    }
}
