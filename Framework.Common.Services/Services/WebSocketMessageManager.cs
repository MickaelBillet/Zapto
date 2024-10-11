using Framework.Core.Base;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Infrastructure.Services
{
    public class WebSocketMessageManager : WebSocketHandler, IWSMessageManager
    {
        #region Properties
		private IInMemoryEvent InMemoryEvent { get; }
        #endregion

        #region Constructor
        public WebSocketMessageManager(IServiceProvider serviceProvider) : base(serviceProvider)
        {
			this.InMemoryEvent = serviceProvider.GetRequiredService<IInMemoryEvent>();
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
            await Task.CompletedTask;
        }

        public override async Task<bool> ReceiveAsync(WebSocket webSocket, WebSocketReceiveResult result, byte[] buffer)
        {
			bool res = true;

            try
			{
                Log.Information("WebSocketMessageManager.ReceiveAsync");

                string received = Encoding.ASCII.GetString(buffer, 0, result.Count).TrimEnd('\0');			
				if (string.IsNullOrEmpty(received) == false)
				{
					Log.Information(received);

					MessageArduino message = MessageArduino.Deserialize(received);
					if (message != null)
					{
                        await this.InMemoryEvent.Publish(message);                        
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
                        await this.OnDisconnected(webSocket);
                    }
				}
				res = false;
			}
            return res;
        }
        #endregion
    }
}
