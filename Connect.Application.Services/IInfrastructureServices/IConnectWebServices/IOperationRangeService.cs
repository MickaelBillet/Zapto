using Connect.Model;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace Connect.Application.Infrastructure
{
    public interface IOperationRangeService
    {
        Task<bool?> DeleteOperationRangeAsync(OperationRange op, CancellationToken token = default);

        Task<bool?> AddOperationRangeAsync(OperationRange op, CancellationToken token = default);

        Task<ObservableCollection<OperationRange>?> GetOperationsRanges(string programId, CancellationToken token = default);

        Task<bool?> DeleteOperationsRanges(string programId, CancellationToken token = default);
    }
}
