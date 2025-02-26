using Framework.Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Connect.WebServer.Helpers
{
    public static class WebSocketHelper
    {
        public static async Task Echo(HttpContext context)
        {
            if (context.WebSockets.IsWebSocketRequest == true)
            {
                using (var webSocket = await context.WebSockets.AcceptWebSocketAsync())
                {
                    var buffer = new byte[1024 * 4];
                    var receiveResult = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                    while (receiveResult.CloseStatus.HasValue == false)
                    {

                        await webSocket.SendAsync(
                            new ArraySegment<byte>(buffer, 0, receiveResult.Count),
                            receiveResult.MessageType,
                            receiveResult.EndOfMessage,
                            CancellationToken.None);

                        receiveResult = await webSocket.ReceiveAsync(
                            new ArraySegment<byte>(buffer), CancellationToken.None);
                    }

                    await webSocket.CloseAsync(
                    receiveResult.CloseStatus.Value,
                    receiveResult.CloseStatusDescription,
                    CancellationToken.None);
                }
            }
        }

        public static async Task WebSocketReception(this IApplicationBuilder builder, HttpContext context)
        {
            using (IServiceScope scope = builder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {            
                IWSMessageManager? messageManager = scope.ServiceProvider.GetRequiredService<IWSMessageManager>();
                if ((messageManager != null) && (context.WebSockets.IsWebSocketRequest == true))
                {
                    using (var webSocket = await context.WebSockets.AcceptWebSocketAsync())
                    {
                        try
                        {
                            if (messageManager.OnConnected(webSocket))
                            {
                                Log.Information("Arduino connected");
                            }

                            var buffer = new byte[1024 * 4];
                            while (webSocket.State == WebSocketState.Open)
                            {
                                WebSocketReceiveResult receiveResult = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                                if (receiveResult.CloseStatus.HasValue == false)
                                {
                                    await messageManager.ReceiveAsync(webSocket, receiveResult, buffer);
                                }
                            }

                            Log.Information("Arduino connected");
                            await messageManager.OnDisconnected(webSocket);
                        }
                        catch (Exception ex)
                        {
                            if (webSocket.State == WebSocketState.Open)
                            {
                                await webSocket.CloseAsync(WebSocketCloseStatus.EndpointUnavailable, ex.Message, CancellationToken.None);
                            }
                        }
                    }
                }
            }
        }
    }
}
