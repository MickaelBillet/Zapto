using Connect.Application.Infrastructure;
using Connect.Model;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace Zapto.Component.Services
{
    public class SignalRService : ISignalRService
    {
        private static readonly string UrlHubConnection = @"/signalr";

        #region Property
        protected HubConnection? HubConnection { get; set; }
        private IConfiguration Configuration { get; }
        public bool IsConnected
        {
            get
            {
                return (this.HubConnection?.State == HubConnectionState.Connected);
            }
        }

        public bool IsDisconnected
        {
            get
            {
                return (this.HubConnection?.State == HubConnectionState.Disconnected);
            }
        }
        #endregion

        #region Constructor
        public SignalRService(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }
        #endregion

        #region Method

        public async Task<bool> StartAsync(string locationId, 
                                            Action<PlugStatus>? actionPlug, 
                                            Action<RoomStatus>? actionRoom, 
                                            Action<SensorStatus>? actionSensor, 
                                            Action<NotificationStatus>? actionNotification)
        {              
            try
            {
                if (this.IsConnected == false)
                {
                    this.HubConnection = new HubConnectionBuilder()
                                                .WithUrl(url: $"{this.Configuration["ProtocolSignalR"]}://{this.Configuration["BackEndUrl"]}" +
                                                                $":{this.Configuration["PortHttpsConnect"]}{this.Configuration["PathBaseSignalR"]}" +
                                                                $"{SignalRService.UrlHubConnection}")
                                                .WithAutomaticReconnect(new[] { TimeSpan.Zero, TimeSpan.Zero, TimeSpan.FromSeconds(10) })
                                                .Build();

                    this.HubConnection.Closed += async (error) =>
                    {
                        await Task.Delay(new Random().Next(1, 5) * 1000);
                        await this.HubConnection.StartAsync();
                    };

                    this.HubConnection.Reconnecting += error =>
                    {
                        // Notify users the connection was lost and the client is reconnecting.
                        // Start queuing or dropping messages.
                        return Task.CompletedTask;
                    };

                    int counter = 0;

                    while ((this.IsDisconnected == false) && (counter < 5)) 
                    {
                        await Task.Delay(100);
                        counter++;
                    }

                    if (this.IsDisconnected == true)
                    {
                        this.ProcessReceive(actionPlug, actionRoom, actionSensor);
                        await this.HubConnection.StartAsync();
                        counter = 0;

                        while ((this.IsConnected == false) && (counter < 5))
                        {
                            await Task.Delay(100);
                            counter++;
                        }

                        if (this.IsConnected == true)
                        {
                            await this.HubConnection.InvokeAsync("AddToLocation", locationId);
                        }
                        else
                        {
                            throw new InvalidOperationException("InvokeAsync");
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException("StartAsync");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Releases all resource used by the <see cref="T:Connect.Mobile.Services.SignalRService"/> object.
        /// </summary>
        /// <remarks>Call <see cref="Dispose"/> when you are finished using the
        /// <see cref="T:Connect.Mobile.Services.SignalRService"/>. The <see cref="Dispose"/> method leaves the
        /// <see cref="T:Connect.Mobile.Services.SignalRService"/> in an unusable state. After calling
        /// <see cref="Dispose"/>, you must release all references to the
        /// <see cref="T:Connect.Mobile.Services.SignalRService"/> so the garbage collector can reclaim the memory that
        /// the <see cref="T:Connect.Mobile.Services.SignalRService"/> was occupying.</remarks>
        public void Dispose()
        {
            if (this.HubConnection != null)
            {
                Task.Run(async () =>
                {
                    await this.HubConnection.DisposeAsync();
                });
            }
        }

        private void ProcessReceive(Action<PlugStatus>? actionPlug, Action<RoomStatus>? actionRoom, Action<SensorStatus>? actionSensor)
        {
            this.HubConnection?.On<PlugStatus>(PlugStatus.Name, (PlugStatus status) =>
            {
                if (actionPlug != null)
                {
                    actionPlug(status);
                }
            });

            this.HubConnection?.On<RoomStatus>(RoomStatus.Name, (RoomStatus status) =>
            {
                if (actionRoom != null)
                {
                    actionRoom(status);
                }

            });

            this.HubConnection?.On<SensorStatus>(SensorStatus.Name, (SensorStatus status) =>
            {
                if (actionSensor != null)
                {
                    actionSensor(status);
                }
            });
        }
        #endregion
    }
}
