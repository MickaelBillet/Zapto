using System.Net.WebSockets;
using System.Threading.Tasks;

namespace Framework.Infrastructure.Services
{
    public interface IWSMessageManager
	{
		Task HandleErrorAsync(WebSocket socket);
		Task<bool> ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer);
		Task<bool> SendMessageAsync(string socketId, string message);
		Task<bool> OnDisconnected(WebSocket socket);
		bool OnConnected(WebSocket socket);
		Task<int> SendMessageToAllAsync(string message);
    }
}
