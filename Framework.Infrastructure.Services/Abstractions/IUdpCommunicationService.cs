using System.Threading.Tasks;

namespace Framework.Infrastructure.Services
{
    public interface IUdpCommunicationService
    {
        Task<int> SendMessage(string message, int port, string address);
        Task<byte[]> StartReception(int port);
    }
}
