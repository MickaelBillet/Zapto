using Connect.Model;
using System.Threading.Tasks;

namespace Connect.Application
{
    public interface IApplicationSensorServices
	{
		Task<int?> Notify(Sensor sensor);
		Task<SensorData?> ReceiveDataAsync();
		Task<SensorEvent?> ReceiveEventAsync();
		Task SendEventToClientAsync(string locationId, Sensor sensor);
		Task<bool?> Leak(string? sensorId, int leakStatus);
		Task NotifySensorLeak(string locationId, Room room);
    }
}
