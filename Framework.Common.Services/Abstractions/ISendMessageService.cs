using System.Threading.Tasks;

namespace Framework.Common.Services
{
    public interface ISendMessageService
    {
        Task<bool> SendMessageAsync(string message);
    }
}
