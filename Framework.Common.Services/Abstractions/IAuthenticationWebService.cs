using Framework.Core.Base;
using System.Threading.Tasks;

namespace Framework.Infrastructure.Services
{
    public interface IAuthenticationWebService
	{
        Task<bool> GetTokenAsync(string url, string username, string password, string client_id, string client_secret);
        Task<bool> RefreshAccessToken();
        Token? Token { get; }
    }
}
