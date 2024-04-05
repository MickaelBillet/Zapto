using Connect.Model;
using Framework.Core.Base;

namespace Connect.Data
{
    public interface ISupervisorCacheLocation : ISupervisorCache
    {
        public Task<IEnumerable<Location>> GetLocations();
        public Task<Location> GetLocation(string id);
        public Task<ResultCode> AddLocation(Location location);
    }
}
