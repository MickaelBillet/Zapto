using Connect.Model;
using System.Threading.Tasks;

namespace Connect.Application
{
    public interface IApplicationClientAppServices
    {
        Task<bool?> Save(ClientApp clientApp);
        Task<ClientApp?> GetClientAppFromToken(string token);
        Task<bool?> Delete(string token);
    }
}