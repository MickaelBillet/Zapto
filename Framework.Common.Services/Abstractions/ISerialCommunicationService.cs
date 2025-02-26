using System.Threading.Tasks;

namespace Framework.Common.Services
{
    public interface ISerialCommunicationService : ISendMessageService
    {
        void Dispose();
        bool Receive();
       // Task<bool> SendMessageAsync(string message);
    }
}