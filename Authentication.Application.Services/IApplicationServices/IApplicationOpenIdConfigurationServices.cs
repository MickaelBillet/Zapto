using Authentication.Model;
using System.Threading.Tasks;

namespace Authentication.Application
{
    public interface IApplicationOpenIdConfigurationServices
    {
        Task<OpenIdConfiguration?> GetOpenIdConfiguration();
    }
}
