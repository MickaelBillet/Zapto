using AirZapto.Model.Healthcheck;
using System.Threading.Tasks;

namespace AirZapto.Application
{
    public interface IApplicationHealthCheckAirZaptoServices
	{
        Task<HealthCheckAirZapto?> GetHealthCheckAirZapto();
    }
}
