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

            //while (receiveResult.CloseStatus.HasValue == false)
            //{

            //        await webSocket.SendAsync(
            //            new ArraySegment<byte>(buffer, 0, receiveResult.Count),
            //            receiveResult.MessageType,
            //            receiveResult.EndOfMessage,
            //            CancellationToken.None);

            //        receiveResult = await webSocket.ReceiveAsync(
            //            new ArraySegment<byte>(buffer), CancellationToken.None);
            //}

            await webSocket.CloseAsync(
            receiveResult.CloseStatus.Value,
            receiveResult.CloseStatusDescription,
            CancellationToken.None);
        }
    }
}
