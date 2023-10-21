using Connect.Model;
using System.Threading.Tasks;

namespace Connect.Application.Infrastructure
{
    public interface IConnectedObjectService
    {
        Task<ConnectedObject?> GetConnectedObject(string objectId);
    }
}
