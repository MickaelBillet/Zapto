using Framework.Core.Base;
using Framework.Infrastructure.Services;
using Serilog;
using System.Net.WebSockets;
using System.Text;

namespace AirZapto.WebServer.Services
{
    public class WebSocketMessageManager : WebSocketHandler, IWSMessageManager
    {
		#region Properties

        #endregion

        #region Constructor
        public WebSocketMessageManager(WebSockerService webSocketConnectionManager, IServiceProvider serviceProvider) : base(webSocketConnectionManager)
        {
		
		}
		#endregion

		#region Methods
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
			 
		public override async Task<bool> ReceiveAsync(WebSocket webSocket, WebSocketReceiveResult result, byte[] buffer)
        {
			bool res = true;

            try
			{
                string received = Encoding.ASCII.GetString(buffer, 0, result.Count).TrimEnd('\0');			
				if (!string.IsNullOrEmpty(received))
				{
					Log.Information(received);

					MessageArduino message = MessageArduino.Deserialize(received);
					if (message != null)
					{
						
                    }
				}
			}
			catch (Exception ex)
			{
				Log.Error(ex.Message);

				if (webSocket != null)
				{
					string idSocket = this.WebSocketConnectionManager.GetId(webSocket);
					if (string.IsNullOrEmpty(idSocket) == false)
					{
                        res = await this.OnDisconnected(webSocket);
                    }
				}
			}
            return res;
        }

        #endregion
    }
}
