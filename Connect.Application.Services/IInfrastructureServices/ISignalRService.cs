using Connect.Model;
using System;
using System.Threading.Tasks;

namespace Connect.Application.Infrastructure
{
    public interface ISignalRService
    {
        static string Group { get; set; } = string.Empty;
        void Dispose();
        Task<bool> StartAsync(string locationId, Action<PlugStatus>? actionPlug, Action<RoomStatus>? actionRoom, Action<SensorStatus>? actionSensor, Action<NotificationStatus>? actionNotification);
    }
}
