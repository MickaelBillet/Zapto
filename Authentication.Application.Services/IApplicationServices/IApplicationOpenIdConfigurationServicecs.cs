using Authentication.Model;
using System.Threading.Tasks;

namespace Authentication.Application
{
    public interface IApplicationOpenIdConfigurationServicecs
    {
        Task<OpenIdConfiguration?> GetHealthCheckAirZapto();
    }
}
