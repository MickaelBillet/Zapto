using Connect.Model;
using System;
using System.Threading.Tasks;

namespace Connect.Application.Infrastructure
{
    public interface ISignalRService
    {
        static string Group { get; set; } = string.Empty;
        void Dispose();
        Task<bool> StartAsync(string locationId, 
                                Func<PlugStatus, Task>? actionPlug, 
                                Func<RoomStatus, Task>? actionRoom, 
                                Func<SensorStatus, Task>? actionSensor, 
                                Func<NotificationStatus, Task>? actionNotification);
    }
}
