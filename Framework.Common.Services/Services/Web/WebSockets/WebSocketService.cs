using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

#nullable disable

namespace Framework.Infrastructure.Services
{
    public class WebSocketService : IWebSocketService
    {
        private ConcurrentDictionary<string, WebSocket> Sockets = new ConcurrentDictionary<string, WebSocket>();

        public WebSocket GetSocketById(string id)
        {
            return Sockets.FirstOrDefault(p => p.Key == id).Value;
        }

        public ConcurrentDictionary<string, WebSocket> GetAll()
        {
            return Sockets;
        }

        public string GetId(WebSocket socket)
        {
            return Sockets.FirstOrDefault(p => p.Value == socket).Key;
        }
        public bool AddSocket(WebSocket socket)
        {
            return Sockets.TryAdd(CreateConnectionId(), socket);
        }

        public async Task<bool> RemoveSocket(string id)
        {
            bool isdisconnected = false;
            if (Sockets.TryRemove(id, out WebSocket socket) == true)
            {
                await socket.CloseAsync(closeStatus: WebSocketCloseStatus.NormalClosure,
                                        statusDescription: "Closed by the ConnectionManager",
                                        cancellationToken: CancellationToken.None);

                isdisconnected = true;
            }
            return isdisconnected;
        }

        private string CreateConnectionId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
