using Connect.Application;
using Connect.Model;
using Framework.Core.Base;
using Framework.Infrastructure.Services;
using InMemoryEventBus.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Net.WebSockets;
using System.Text;

namespace AirZapto.WebServer.Services
{
    public class WebSocketMessageManager : WebSocketHandler, IWSMessageManager
    {
        #region Properties
		private IProducer<MessageArduino> Producer { get; }
        #endregion

        #region Constructor
        public WebSocketMessageManager(IServiceProvider serviceProvider) : base(serviceProvider)
        {
			this.Producer = serviceProvider.GetRequiredService<IProducer<MessageArduino>>();
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
                        //if (message.Header == ConnectConstants.PlugStatus)
                        //{
                        //	await this.ApplicationPlugServices.ReadStatus(message.Payload);
                        //}
                        //else if (message.Header == ConnectConstants.SensorData)
                        //{
                        //	await this.ApplicationSensorServices.ReadData(message.Payload);
                        //}
                        //else if (message.Header == ConnectConstants.SensorEvent)
                        //{
                        //	await this.ApplicationSensorServices.ReadEvent(message.Payload);
                        //}
                        //else if (message.Header == ConnectConstants.ServerIotStatus)
                        //{
                        //	await this.ApplicationServerIotServices.ReadStatus(message.Payload);
                        //}

                        var publishEventsFn = async (int i) =>
                        {
                            var @event = new Event<MessageArduino>(message);

                            await this.Producer.Publish(@event).ConfigureAwait(false);
                        };

                        await publishEventsFn.Invoke(0);
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
