using Connect.Application.Infrastructure;
using Connect.Model;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Threading.Tasks;

namespace Connect.Mobile.Services
{
    public class SignalRService : ISignalRService
    {
        public static readonly string UrlHubConnection = @"/signalr";

        #region Property
        public static string Group { get; set; }
        protected HubConnection HubConnection { get; set; }
        private IConfiguration Configuration { get; }
        public bool IsConnected
        {
            get
            {
                return HubConnection?.State == HubConnectionState.Connected ? true : false;
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
                                            Func<PlugStatus, Task> actionPlug,
                                            Func<RoomStatus, Task> actionRoom,
                                            Func<SensorStatus, Task> actionSensor,
                                            Func<NotificationStatus, Task> actionNotification)
        {
            try
            {
                if (this.IsConnected == false)
                {
                    this.HubConnection = new HubConnectionBuilder()
                                                .WithUrl(url: $"{Configuration["ProtocolSignalR"]}://{Configuration["BackEndUrl"]}" +
                                                                $":{Configuration["PortHttp"]}{Configuration["PathBaseSignalR"]}" +
                                                                $"{UrlHubConnection}")
                                                .WithAutomaticReconnect(new[] { TimeSpan.Zero, TimeSpan.Zero, TimeSpan.FromSeconds(10) })
                                                .Build();

                    this.HubConnection.Closed += async (error) =>
                    {
                        await Task.Delay(new Random().Next(0, 5) * 1000);
                        await this.HubConnection.StartAsync();
                    };

                    this.HubConnection.Reconnecting += error =>
                    {
                        Log.Warning("Reconnection : " + this.HubConnection.State.ToString());

                        // Notify users the connection was lost and the client is reconnecting.
                        // Start queuing or dropping messages.
                        return Task.CompletedTask;
                    };

                    ProcessReceive(actionPlug, actionRoom, actionSensor, actionNotification);

                    await this.HubConnection.StartAsync();
                    await this.HubConnection.InvokeAsync(ConnectConstants.SignalR_AddToLocation, locationId);
                }
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message);
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
        public async void Dispose()
        {
            if (this.HubConnection != null)
            {
                await this.HubConnection.DisposeAsync();
                this.HubConnection = null;
            }
        }

        private void ProcessReceive(Func<PlugStatus, Task> actionPlug, 
                                    Func<RoomStatus, Task> actionRoom, 
                                    Func<SensorStatus, Task> actionSensor, 
                                    Func<NotificationStatus, Task> actionNotification)
        {
            this.HubConnection.On(PlugStatus.Name, (PlugStatus status) =>
            {
                if (actionPlug != null)
                {
                    actionPlug(status);
                }
            });

            this.HubConnection.On(RoomStatus.Name, (RoomStatus status) =>
            {
                if (actionRoom != null)
                {
                    actionRoom(status);
                }
            });

            this.HubConnection.On(SensorStatus.Name, (SensorStatus status) =>
            {
                if (actionSensor != null)
                {
                    actionSensor(status);
                }
            });

            this.HubConnection.On(NotificationStatus.Name, (NotificationStatus status) =>
            {
                if (actionNotification != null)
                {
                    actionNotification(status);
                }
            });
        }
        #endregion
    }
}
