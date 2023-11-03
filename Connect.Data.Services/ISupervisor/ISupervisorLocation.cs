using Connect.Model;
using Framework.Core.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Connect.Data
{
    public interface ISupervisorLocation
    {
        Task<ResultCode> AddLocation(Location location);
        Task<IEnumerable<Location>> GetLocations();
        Task<Location> GetLocation(string id);
    }
}
