using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Infrastructure.Services
{
    public interface IWebSocketService
    {
        WebSocket GetSocketById(string id);
        ConcurrentDictionary<string, WebSocket> GetAll();
        string GetId(WebSocket socket);
        Task<bool> RemoveSocket(string id);
        bool AddSocket(WebSocket socket);
    }
}
