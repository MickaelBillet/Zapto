using Connect.Model;
using System.Threading.Tasks;

namespace Connect.Application
{
    public interface IApplicationSensorServices
	{
		Task<int?> Notify(Sensor sensor);
		Task SendEventToClientAsync(string locationId, Sensor sensor);
		Task NotifySensorLeak(string locationId, Room room);
		Task ReadDataAsync(string data);
		Task ReadEventAsync(string data);
    }
}
