using Connect.Model;
using Framework.Core.Base;

namespace Connect.Data
{
    public interface ISupervisorCacheProgram : ISupervisorCache
    {
        public Task<ResultCode> AddProgram(Program program);
        public Task<Program> GetProgram(string id);
    }
}
