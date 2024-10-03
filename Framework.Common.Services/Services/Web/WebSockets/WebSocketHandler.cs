using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Infrastructure.Services
{
    public abstract class WebSocketHandler
    {
        #region Properties

        public WebSockerService WebSocketConnectionManager { get; private set; }

        #endregion

        #region Constructor
        public WebSocketHandler(WebSockerService webSocketConnectionManager)
        {
            this.WebSocketConnectionManager = webSocketConnectionManager;
        }
        #endregion

        #region Methods

        public virtual bool OnConnected(WebSocket socket)
        {
            return this.WebSocketConnectionManager.AddSocket(socket);
        }
        public virtual async Task<bool> OnDisconnected(WebSocket socket)
        {
            return await this.WebSocketConnectionManager.RemoveSocket(this.WebSocketConnectionManager.GetId(socket));
        }
        public async Task<bool> SendMessageAsync(WebSocket socket, string message)
        {
            if (socket == null || socket.State != WebSocketState.Open)
                return false;

            await socket.SendAsync(buffer: new ArraySegment<byte>(array: Encoding.ASCII.GetBytes(message),
                                                                  offset: 0,
                                                                  count: message.Length),
                                   messageType: WebSocketMessageType.Text,
                                   endOfMessage: true,
                                   cancellationToken: CancellationToken.None);

            return socket.State == WebSocketState.Open ? true : false;
        }
        public async Task<bool> SendMessageAsync(string socketId, string message)
        {
            return await this.SendMessageAsync(this.WebSocketConnectionManager.GetSocketById(socketId), message);
        }
        public async Task SendMessageToAllAsync(string message)
        {
            foreach (var pair in this.WebSocketConnectionManager.GetAll())
            {
                if (pair.Value.State == WebSocketState.Open)
                    await this.SendMessageAsync(pair.Value, message);
            }
        }
        public abstract Task<bool> ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer);
        public abstract Task HandleErrorAsync(WebSocket socket);

        #endregion
    }
}
