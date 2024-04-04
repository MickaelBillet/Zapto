using Connect.Model;
using Framework.Core.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Connect.Data
{
    public interface ISupervisorCondition
    {
        Task<Condition> GetCondition(string id);
        Task<ResultCode> AddCondition(Condition condition);
        Task<ResultCode> UpdateCondition(string id, Condition condition);
        Task<ResultCode> DeleteCondition(Condition condition);
        Task<ResultCode> ConditionExists(string id);
    }
}
