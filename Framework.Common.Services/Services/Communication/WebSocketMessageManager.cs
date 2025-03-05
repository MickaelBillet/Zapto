using Framework.Core.Base;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace Framework.Infrastructure.Services
{
    public class WebSocketMessageManager : WebSocketHandler, IWSMessageManager
    {
        private readonly IEventBusProducerConnect _eventBusProducer;

        #region Constructor
        public WebSocketMessageManager(IServiceProvider serviceProvider) : base(serviceProvider)
        {
			_eventBusProducer = serviceProvider.GetRequiredService<IEventBusProducerConnect>();
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

				if (result.Count > 0)
				{
					MessageArduino message = MessageArduino.Deserialize(buffer, result.Count);
					if (message != null)
					{
                        await _eventBusProducer.Publish(message);                        
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
