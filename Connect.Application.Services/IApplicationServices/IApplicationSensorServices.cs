using Connect.Model;
using System.Threading.Tasks;

namespace Connect.Application
{
    public interface IApplicationSensorServices
	{
		Task<int?> Notify(Sensor sensor);
		Task SendEventToClient(string locationId, Sensor sensor);
		Task NotifySensorLeak(string locationId, Room room);
		Task ReadData(SensorData? sensorData);
		Task ReadEvent(SensorEvent? sensorEvent);
        Task<bool?> Leak(string? sensorId, int leakStatus);
    }
}
