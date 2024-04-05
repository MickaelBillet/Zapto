using Connect.Model;
using Framework.Core.Base;

namespace Connect.Data
{
    public interface ISupervisorCacheConnectedObject : ISupervisorCache
    {
        public Task<ConnectedObject> GetConnectedObject(string? id);
        public Task<IEnumerable<ConnectedObject>> GetConnectedObjects();
        public Task<ResultCode> AddConnectedObject(ConnectedObject obj);
    }
}
