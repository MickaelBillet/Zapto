using Connect.Model;
using Framework.Core.Base;

namespace Connect.Data
{
    public interface ISupervisorCacheCondition : ISupervisorCache
    {
        public Task<Condition> GetCondition(string id);
        public Task<ResultCode> AddCondition(Condition condition);
        public Task<ResultCode> UpdateCondition(string id, Condition condition);
        public Task<ResultCode> DeleteCondition(Condition condition);
    }
}
