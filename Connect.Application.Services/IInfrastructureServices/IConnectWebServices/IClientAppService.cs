using Connect.Model;
using System.Threading;
using System.Threading.Tasks;

namespace Connect.Application.Infrastructure
{
    public interface IClientAppService
    {
        Task<bool?> AddClientAppAsync(ClientApp clientApp, CancellationToken token = default);
        Task<ClientApp?> GetClientAppAsync(string? firebaseToken, CancellationToken token = default);
        Task<bool?> DeleteClientAppAsync(string? firebaseToken, CancellationToken token = default);
    }
}