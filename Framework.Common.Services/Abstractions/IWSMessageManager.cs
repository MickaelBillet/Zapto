using Framework.Common.Services;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace Framework.Infrastructure.Services
{
    public interface IWSMessageManager : ISendMessageService
	{
        Task HandleErrorAsync(WebSocket socket);
        Task<bool> ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer);
        Task<bool> SendMessageAsync(string socketId, string message);
        Task<bool> OnDisconnected(WebSocket socket);
        bool OnConnected(WebSocket socket);
    }
}
