using Connect.Application.Infrastructure;
using Connect.Model;
using Microsoft.AspNetCore.SignalR;
using Serilog;


namespace Connect.WebServer.Services
{
    public class SignalRConnectService : ISignalRConnectService
    {
        #region Property
        private IHubContext<ConnectHub> HubContext { get; set; }
        #endregion

        #region Constructor
        public SignalRConnectService(IHubContext<ConnectHub> context)
        {
            this.HubContext = context;
        }
        #endregion

        #region Method
        public async Task SendPlugStatusAsync(string locationId, Plug plug)
        {
            try
            {
                IClientProxy proxy = this.HubContext.Clients.Group(locationId);
                if (proxy != null)
                {
                    await proxy.SendAsync(PlugStatus.Name, new PlugStatus
                    {
                        PlugId = plug.Id,
                        Status = plug.Status,
                        WorkingDuration = plug.WorkingDuration,
                        Order = plug.Order,
                        OnOff = plug.OnOff,
                        Mode = plug.Mode,
                    });
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
            }
        }

        public async Task SendRoomStatusAsync(string locationId, Room room)
        {
            try
            {
                IClientProxy proxy = this.HubContext.Clients.Group(locationId);
                if (proxy != null)
                {
                    await proxy.SendAsync(RoomStatus.Name, new RoomStatus
                    {
                        RoomId = room.Id,
                        Temperature = room.Temperature,
                        Pressure = room.Pressure,
                        Humidity = room.Humidity,
                    });
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
            }
        }

        public async Task SendSensorStatusAsync(string locationId, Sensor sensor)
        {
            try
            {
                IClientProxy proxy = this.HubContext.Clients.Group(locationId);
                if (proxy != null)
                {
                    await proxy.SendAsync(SensorStatus.Name, new SensorStatus
                    {
                        SensorId = sensor.Id,
                        Temperature = sensor.Temperature,
                        Pressure = sensor.Pressure,
                        Humidity = sensor.Humidity,
                        IsRunning = (byte)sensor.IsRunning,
                        LeakDetected = (byte)sensor.LeakDetected,
                        RoomId = sensor.RoomId,
                    });
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
            }
        }

        public async Task SendAlertAsync(string locationId, string title, string body)
        {
            try
            {
                IClientProxy proxy = this.HubContext.Clients.Group(locationId);
                if (proxy != null)
                {
                    await proxy.SendAsync(NotificationStatus.Name, new NotificationStatus
                    {
                        Title = title,
                        Body = body
                    });
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
            }
        }
        #endregion
    }
}
