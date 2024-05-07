using Connect.Model;
using Framework.Core.Base;

namespace Connect.Data
{
    public interface ISupervisorLocation : ISupervisor
    {
        Task<ResultCode> AddLocation(Location location);
        Task<IEnumerable<Location>> GetLocations();
        Task<Location> GetLocation(string id);
    }
}
