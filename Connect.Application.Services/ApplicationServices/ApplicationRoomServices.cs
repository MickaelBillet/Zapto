using Connect.Application.Infrastructure;
using Connect.Model;
using Framework.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Connect.Application.Services
{
    internal sealed class ApplicationRoomServices : IApplicationRoomServices
	{
		#region Services
		private IRoomService? RoomService { get; }
		private ISignalRConnectService? SignalRConnectService { get; }
        private IAlertService? AlertService { get; }
        #endregion

        #region Constructor

        public ApplicationRoomServices(IServiceProvider serviceProvider)
		{
			this.RoomService = serviceProvider.GetService<IRoomService>();
			this.SignalRConnectService = serviceProvider.GetService<ISignalRConnectService>();
            this.AlertService = AlertServiceFactory.CreateAlerteService(serviceProvider, ServiceAlertType.Mail);
        }

        #endregion

        #region Methods
        public IObservable<Room>? GetRoom(string roomId)
		{
			return this.RoomService?.GetRoom(roomId, true);
		}

		public async Task<IEnumerable<Room>?> GetRoomsAsync(string? locationId)
		{
			return (this.RoomService != null) ? (await this.RoomService.GetRooms(locationId)) : null;
		}

        public async Task SendDataToClientAsync(string locationId, Room room)
        {
			if (this.SignalRConnectService != null)
			{
				await this.SignalRConnectService.SendRoomStatusAsync(locationId, room);
			}

            Log.Warning("SendDataToClient : " + room.Name + " Temperature : " + room.Temperature + " Humidity : " + room.Humidity);
        }

        public async Task NotifiyRoomCondition(string locationId, IEnumerable<Notification> notifications, Room room)
        {            
            if (notifications?.Count() > 0)
            {
                foreach (Notification notification in notifications)
                {
                    if (notification.IsEnabled)
                    {
                        if (notification.Parameter == ParameterType.Temperature)
                        {
                            await this.NotifiyTemperatureCondition(notification, locationId, room);
                        }
                        else if (notification.Parameter == ParameterType.Humidity)
                        {
                            await this.NotifiyHumidityCondition(notification, locationId, room);
                        }
                    }
                }
            }
        }

        private async Task NotifiyHumidityCondition(Notification notification, string locationId, Room room)
        {
            if (this.AlertService != null)
            {
                if (notification.Sign == SignType.Upper)
                {
                    if (room.Humidity > notification.Value)
                    {
                        if (notification.ConfirmationFlag >= Notification.LIMIT)
                        {
                            await this.AlertService.SendAlertAsync(locationId,
                                                                    room.Name,
                                                                    $"Avertissement Humidité de {room.Humidity?.ToString("D")}%" +
                                                                    $" (Supérieure à {notification.Value}%) pour {room.Name}");
                        }
                        else
                        {
                            notification.ConfirmationFlag++;
                        }
                    }
                    else
                    {
                        notification.ConfirmationFlag = 0;
                    }
                }
                else if (notification.Sign == SignType.Lower)
                {
                    if (room.Humidity < notification.Value)
                    {
                        if (notification.ConfirmationFlag >= Notification.LIMIT)
                        {
                            await this.AlertService.SendAlertAsync(locationId,
                                                                        room.Name,
                                                                        $"Avertissement Humidité de {room.Humidity?.ToString("D")}%" +
                                                                        $" (Inférieure à {notification.Value}%) pour  {room.Name}");

                        }
                        else
                        {
                            notification.ConfirmationFlag++;
                        }
                    }
                    else
                    {
                        notification.ConfirmationFlag = 0;
                    }
                }
            }
        }

        private async Task NotifiyTemperatureCondition(Notification notification, string locationId, Room room)
        {
            if (this.AlertService != null)
            {
                if (notification.Sign == SignType.Upper)
                {
                    if (room.Temperature > notification.Value)
                    {
                        if (notification.ConfirmationFlag >= Notification.LIMIT)
                        {
                            await this.AlertService.SendAlertAsync(locationId,
                                                                    room.Name,
                                                                    $"Avertissement Température de {room.Temperature?.ToString("F1")}°C" +
                                                                    $" (Supérieure à {notification.Value}°C) pour {room.Name}");
                        }
                        else
                        {
                            notification.ConfirmationFlag++;
                        }
                    }
                    else
                    {
                        notification.ConfirmationFlag = 0;
                    }
                }
                else if (notification.Sign == SignType.Lower)
                {
                    if (room.Temperature < notification.Value)
                    {
                        if (notification.ConfirmationFlag >= Notification.LIMIT)
                        {
                            await this.AlertService.SendAlertAsync(locationId,
                                                                    room.Name,
                                                                    $"Avertissement Température de {room.Temperature?.ToString("F1")}°C" +
                                                                    $" (Inférieure à {notification.Value}°C) pour  {room.Name}");
                        }
                        else
                        {
                            notification.ConfirmationFlag++;
                        }
                    }
                    else
                    {
                        notification.ConfirmationFlag = 0;
                    }
                }
            }
        }
        #endregion
    }
}
