﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Infrastructure.Services
{
    public class WebSocketHelper
    {
        public static async Task Echo(WebSocket webSocket)
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

        public static async Task Process(IServiceProvider serviceProvider, HttpContext context)
        {
            IWSMessageManager messageManager = serviceProvider.GetRequiredService<IWSMessageManager>();
            if (context.WebSockets.IsWebSocketRequest == true)
            {
                using (var webSocket = await context.WebSockets.AcceptWebSocketAsync())
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
                    
                    await messageManager.OnDisconnected(webSocket);
                }
            }
        }
    }
}