using Connect.Model;
using System.Threading.Tasks;

namespace Connect.Application
{
	public interface IApplicationPlugServices
	{
		Task<bool?> ChangeMode(Plug plug);
		Task<bool?> SwitchOnOff(Plug plug);
		Task<int> SendCommandAsync(Plug plug);
		Task SendStatusToClientAsync(string locationId, Plug plug);
		Task NotifyPlugStatus(string locationId, Plug plug, string nameConnectedObject);
		Task ReadStatusAsync(string data);
    }
}
