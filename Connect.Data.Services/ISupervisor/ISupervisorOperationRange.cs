using Connect.Model;
using Framework.Core.Base;

namespace Connect.Data
{
    public interface ISupervisorOperationRange : ISupervisor
    {
        Task<IEnumerable<OperationRange>> GetOperationRanges();
        Task<OperationRange> GetOperationRange(string id);
        Task<IEnumerable<OperationRange>> GetOperationRanges(string programId);
        Task<ResultCode> AddOperationRange(OperationRange operationRange);
        Task<ResultCode> DeleteOperationRange(OperationRange operationRange);
        Task<ResultCode> DeleteOperationRanges(IEnumerable<OperationRange> operationRanges);
    }
}
