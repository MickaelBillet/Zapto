using System.Threading.Tasks;

namespace Connect.Application
{
    public interface IApplicationServerIotServices
	{
        Task ReadStatusAsync(string data);
    }
}
