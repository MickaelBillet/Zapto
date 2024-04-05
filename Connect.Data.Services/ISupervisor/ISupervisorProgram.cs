using Connect.Model;
using Framework.Core.Base;

namespace Connect.Data
{
    public interface ISupervisorProgram
    {
        Task<ResultCode> AddProgram(Program program);
        Task<ResultCode> ProgramExists(string id);
        Task<Program> GetProgram(string id);
        Task<IEnumerable<Program>> GetPrograms();
    }
}
