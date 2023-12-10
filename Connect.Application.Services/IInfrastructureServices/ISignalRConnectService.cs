using Connect.Model;
using Framework.Infrastructure.Services;
using System.Threading.Tasks;

namespace Connect.Application.Infrastructure
{
    public interface ISignalRConnectService : IAlertService
    {
        Task SendPlugStatusAsync(string locationId, Plug plug);
        Task SendRoomStatusAsync(string locationId, Room room);
        Task SendSensorStatusAsync(string locationId, Sensor sensor);
    }
}
