using Connect.Model;
using Framework.Core.Base;

namespace Connect.Data
{
    public interface ISupervisorCacheOperationRange : ISupervisorCache
    {
        public Task<OperationRange> GetOperationRange(string id);
        public Task<IEnumerable<OperationRange>> GetOperationRanges(string programId);
        public Task<ResultCode> AddOperationRange(OperationRange operationRange);
        public Task<ResultCode> DeleteOperationRange(OperationRange operationRange);
        public Task<ResultCode> DeleteOperationRanges(IEnumerable<OperationRange> operationRanges);
    }
}
