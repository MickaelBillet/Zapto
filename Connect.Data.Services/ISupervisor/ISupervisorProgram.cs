using Connect.Model;
using Framework.Core.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Connect.Data
{
    public interface ISupervisorProgram
    {
        Task<ResultCode> AddProgram(Program program);
        Task<ResultCode> ProgramExists(string id);
        Task<IEnumerable<Program>> GetPrograms();
        Task<Program> GetProgram(string id);
    }
}
