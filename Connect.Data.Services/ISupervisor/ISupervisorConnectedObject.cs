using Connect.Model;
using Framework.Core.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Connect.Data
{
    public interface ISupervisorConnectedObject : ISupervisor
    {
        Task<ConnectedObject> GetConnectedObject(string? id);
        Task<IEnumerable<ConnectedObject>> GetConnectedObjects();
        Task<ResultCode> AddConnectedObject(ConnectedObject obj);
    }
}
