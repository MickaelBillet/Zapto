using System.Threading.Tasks;

namespace Connect.Application
{
    public interface IApplicationServerIotServices
	{
        Task ReadStatus(string data);
    }
}
