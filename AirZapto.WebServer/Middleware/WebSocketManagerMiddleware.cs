using Framework.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace AirZapto.WebServices.Middleware
{
    public class WebSocketManagerMiddleware
    {
        private readonly RequestDelegate _next;

		#region Properties
		private IWSMessageManager WebSocketHandler { get; set; }
		#endregion

		#region Constructor

		public WebSocketManagerMiddleware(RequestDelegate next, IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _next = next;
            this.WebSocketHandler = serviceProvider.GetRequiredService<IWSMessageManager>();
        }

        #endregion

        #region Methods
        public async Task Invoke(HttpContext context)
        {
            if (context.WebSockets.IsWebSocketRequest == false)
                return;

            WebSocket? socket = null;

            try
            {
                socket = await context.WebSockets.AcceptWebSocketAsync();
                if (this.WebSocketHandler.OnConnected(socket))
                {
                    Log.Information("Sensor connected");
                }

                await this.Receive(socket, async (result, buffer) =>
                {
                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        bool res = await this.WebSocketHandler.ReceiveAsync(socket, result, buffer);
                        if (res == false) 
                        {
                            Log.Error("WebSocketHandler.ReceiveAsync Error");
                        }
                        return;
                    }
                    else if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await this.WebSocketHandler.OnDisconnected(socket);
                        return;
                    }
                });
            }
            catch(Exception ex)
			{
                Log.Error(ex.Message);
                await this.WebSocketHandler.HandleErrorAsync(socket);
            }
        }

        private async Task Receive(WebSocket socket, Action<WebSocketReceiveResult, byte[]> handleMessage)
        {
            byte[] buffer = new byte[1024 * 4];

            while (socket.State == WebSocketState.Open)
            {
                WebSocketReceiveResult result = await socket.ReceiveAsync(buffer: new ArraySegment<byte>(buffer),
                                                                            cancellationToken: CancellationToken.None);
                handleMessage(result, buffer);
            }
        }

		#endregion
	}
}
