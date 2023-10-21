using Framework.Core.Domain;
using System.Threading.Tasks;

namespace Framework.Infrastructure.Services
{
    public interface IAuthenticationService
	{
        Task<ZaptoUser> GetAuthenticatedUser();
    }
}
