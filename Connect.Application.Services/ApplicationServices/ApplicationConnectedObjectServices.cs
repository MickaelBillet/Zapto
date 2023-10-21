using Connect.Application.Infrastructure;
using Connect.Model;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Connect.Application.Services
{
    internal class ApplicationConnectedObjectServices : IApplicationConnectedObjectServices
    {
        #region Services
		private IAlertService? AlertService { get; }
        private ISignalRConnectService? SignalRConnectService { get; }  
        #endregion

        #region Constructor
        public ApplicationConnectedObjectServices(IServiceProvider serviceProvider)
		{
            this.AlertService = AlertServiceFactory.CreateAlerteService(serviceProvider, ServiceAlertType.Firebase);
            this.SignalRConnectService = serviceProvider.GetService<ISignalRConnectService>();
        }
        #endregion

        #region Methods
        public async Task SendDataToClientAsync(string locationId, ConnectedObject @object)
        {
            if ((this.SignalRConnectService != null) && (@object.Sensor != null))
            {
                await this.SignalRConnectService.SendSensorStatusAsync(locationId, @object.Sensor);
                Log.Warning("SendDataToClient : " + @object.Name + " Temperature : " + @object.Sensor.Temperature + " Humidity : " + @object.Sensor.Humidity);
			}
		}

        public async Task NotifiyConnectedObjectCondition(IEnumerable<Notification> notifications, Room room, ConnectedObject connectedObject)
        {
            if (this.AlertService != null) 
            { 
                if (notifications?.Count() > 0)
                {
                    foreach (Notification notification in notifications)
                    {
                        if (notification.IsEnabled)
                        {
                            if (notification.Parameter == ParameterType.Temperature)
                            {
                                if (notification.Sign == SignType.Upper)
                                {
                                    if (connectedObject.Sensor?.Temperature > notification.Value)
                                    {
                                        if (notification.ConfirmationFlag >= Notification.LIMIT)
                                        {
                                            await this.AlertService.SendAlertAsync(room.LocationId,
                                                                                            connectedObject.Name,
                                                                                            $"Température de {connectedObject.Sensor.Temperature?.ToString("F1")}°C" +
                                                                                            $" (Supérieure à {notification.Value}°C) pour {connectedObject.Name}");
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
                                    if (connectedObject.Sensor?.Temperature < notification.Value)
                                    {
                                        if (notification.ConfirmationFlag >= Notification.LIMIT)
                                        {
                                            await this.AlertService.SendAlertAsync(room.LocationId,
                                                                                            connectedObject.Name,
                                                                                            $"Avertissement Température de {connectedObject.Sensor.Temperature?.ToString("F1")}°C" +
                                                                                            $" (Inférieure à {notification.Value}°C) pour {connectedObject.Name}");                                          
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
                            else if (notification.Parameter == ParameterType.Humidity)
                            {
                                if (notification.Sign == SignType.Upper)
                                {
                                    if (connectedObject.Sensor?.Humidity > notification.Value)
                                    {
                                        if (notification.ConfirmationFlag >= Notification.LIMIT)
                                        {
                                            await this.AlertService.SendAlertAsync(room.LocationId,
                                                                                            connectedObject.Name,
                                                                                            $"Avertissement Humidité de {connectedObject.Sensor.Humidity?.ToString("D")}%" +
                                                                                            $" (Supérieure à {notification.Value}%) pour {connectedObject.Name}");
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
                                    if (connectedObject.Sensor?.Humidity < notification.Value)
                                    {
                                        if (notification.ConfirmationFlag >= Notification.LIMIT)
                                        {
                                            await this.AlertService.SendAlertAsync(room.LocationId,
                                                                                            connectedObject.Name,
                                                                                            $"Avertissement Humidité de {connectedObject.Sensor.Humidity?.ToString("D")}% " +
                                                                                            $"(Inférieure à {notification.Value}%) pour {connectedObject.Name}");
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
                    }
                }
            }
        }
        #endregion
    }
}
