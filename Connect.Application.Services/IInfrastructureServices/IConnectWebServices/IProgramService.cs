using Connect.Model;
using System.Threading.Tasks;

namespace Connect.Application.Infrastructure
{
    public interface IProgramService
    {
        Task<Program?> GetProgram(string programId);
    }
}
