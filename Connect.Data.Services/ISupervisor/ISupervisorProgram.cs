using Connect.Model;
using Framework.Core.Base;

namespace Connect.Data
{
    public interface ISupervisorProgram : ISupervisor
    {
        Task<ResultCode> AddProgram(Program program);
        Task<Program> GetProgram(string id);
        Task<IEnumerable<Program>> GetPrograms();
    }
}
