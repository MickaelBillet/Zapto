using Connect.Model;
using Framework.Core.Base;

namespace Connect.Data
{
    public interface ISupervisorCacheConfiguration : ISupervisorCache
    {
        public Task<ResultCode> AddConfiguration(Configuration configuration);
    }
}
