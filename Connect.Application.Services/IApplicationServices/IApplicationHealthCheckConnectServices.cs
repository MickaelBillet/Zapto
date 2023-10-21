using Connect.Model.Healthcheck;
using System.Threading.Tasks;

namespace Connect.Application
{
    public interface IApplicationHealthCheckConnectServices
	{
        Task<HealthCheckConnect?> GetHealthCheckConnect();
    }
}
