using System.Threading.Tasks;

namespace Framework.Common.Services
{
    public interface ISendMessageToArduinoService
    {
        Task<int> Send(string json);
    }
}
