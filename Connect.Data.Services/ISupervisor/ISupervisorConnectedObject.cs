using Connect.Model;
using Framework.Core.Base;

namespace Connect.Data
{
    public interface ISupervisorConnectedObject
    {
        Task<ConnectedObject> GetConnectedObject(string? id);
        Task<ResultCode> ConnectedObjectExists(string id);
        Task<IEnumerable<ConnectedObject>> GetConnectedObjects();
        Task<ResultCode> AddConnectedObject(ConnectedObject obj);
    }
}
