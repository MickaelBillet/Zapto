using Connect.Model;
using System.Threading.Tasks;

namespace Connect.Application
{
	public interface IApplicationPlugServices
	{
		Task<bool?> ChangeMode(Plug plug);
		Task<bool?> SwitchOnOff(Plug plug);
		Task<int> SendCommand(Plug plug);
		Task SendStatusToClient(string locationId, Plug plug);
		Task NotifyPlugStatus(string locationId, Plug plug, string nameConnectedObject);
		Task ReadStatus(string data);
    }
}
