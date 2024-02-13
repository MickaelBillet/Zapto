using System.Threading.Tasks;

namespace Framework.Infrastructure.Services
{
    public interface ISendMailWithFileService
    {
        Task Send(string directoryPath);
    }
}