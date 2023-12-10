using System.Threading.Tasks;

namespace Framework.Infrastructure.Services
{
    public interface ISendMailLogFileService
    {
        Task Send(string directoryPath);
    }
}