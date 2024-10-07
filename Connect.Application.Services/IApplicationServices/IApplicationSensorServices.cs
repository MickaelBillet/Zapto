using Connect.Model;
using System.Threading.Tasks;

namespace Connect.Application
{
    public interface IApplicationSensorServices
	{
		Task<int?> Notify(Sensor sensor);
		Task SendEventToClient(string locationId, Sensor sensor);
		Task NotifySensorLeak(string locationId, Room room);
		Task ReadData(string data);
		Task ReadEvent(string data);
    }
}
