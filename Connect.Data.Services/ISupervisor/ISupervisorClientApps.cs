using Connect.Model;
using Framework.Core.Base;

namespace Connect.Data
{
    public interface ISupervisorClientApps
    {
        Task<ResultCode> ClientAppExists(string id);
        Task<IEnumerable<ClientApp>> GetClientApps();
        Task<ClientApp> GetClientApp(string id);
        Task<ResultCode> AddClientApp(ClientApp clientApp);
        Task<ResultCode> DeleteClientApp(ClientApp clientApp);
        Task<ClientApp> GetClientAppFromToken(string token);
    }
}
