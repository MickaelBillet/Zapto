using Connect.Model;
using System.Threading;
using System.Threading.Tasks;

namespace Connect.Application.Infrastructure
{
	public interface IConditionService
    {
        Task<bool?> DeleteConditionAsync(Condition condition, CancellationToken token = default);

        Task<bool?> AddConditionAsync(Condition condition, CancellationToken token = default);

        Task<bool?> UpdateConditionAsync(Condition condition, CancellationToken token = default);
    }
}
