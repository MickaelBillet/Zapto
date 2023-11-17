using Authentication.Model;
using System.Threading;
using System.Threading.Tasks;

namespace Authentication.Application.Infrastructure
{
    public interface IOpenIdConfigurationService
    {
        Task<OpenIdConfiguration?> GetOpenIdConfiguration(CancellationToken token = default);
    }
}
