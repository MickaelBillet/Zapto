using Framework.Core.Base;
using Framework.Infrastructure.Services;
using Serilog;
using System.Net.WebSockets;
using System.Text;

namespace WebApplicationWebSocket.Services
{
    public class WSMessageManager : WebSocketHandler, IWSMessageManager
    {
        #region Properties

        #endregion

        #region Constructor
        public WSMessageManager(WebSocketService webSocketConnectionManager, IServiceProvider serviceProvider, IConfiguration configuration) : base(webSocketConnectionManager)
        {

        }
        #endregion

        #region Methods
        public override async Task<bool> OnDisconnected(WebSocket socket)
        {
            if (socket != null)
            {
                string idSocket = this.WebSocketConnectionManager.GetId(socket);
                if (string.IsNullOrEmpty(idSocket) == false)
                {
                }
            }

            return await base.OnDisconnected(socket);
        }

        public override async Task HandleErrorAsync(WebSocket socket)
        {
            if (socket != null)
            {
                string idSocket = this.WebSocketConnectionManager.GetId(socket);
                if (string.IsNullOrEmpty(idSocket) == false)
                {
                    
                }
            }
        }

        public override async Task<bool> ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            bool res = true;

            try
            {
                string received = Encoding.ASCII.GetString(buffer, 0, result.Count).TrimEnd('\0');
                if (string.IsNullOrEmpty(received) == false)
                {
                    Log.Information(received);

                    MessageArduino message = MessageArduino.Deserialize(received);
                    if (message != null)
                    {
                        if (message.Header == 0)
                        { }
                    }
                }

                return res;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);

                if (socket != null)
                {
                    string idSocket = this.WebSocketConnectionManager.GetId(socket);
                    if (string.IsNullOrEmpty(idSocket) == false)
                    {
                        if (socket.State == WebSocketState.Open)
                        {

                        }
                        else
                        {
                        }
                    }
                }

                return false;
            }
        }
        #endregion
    }
}
