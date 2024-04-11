using Connect.Model;
using Framework.Core.Base;
using Microsoft.Extensions.DependencyInjection;

namespace Connect.Data.Supervisors
{
    public sealed class SupervisorCacheOperationRange : SupervisorCache, ISupervisorOperationRange
    {
        #region Services
        private ISupervisorOperationRange Supervisor { get; }
        #endregion

        #region Constructor
        public SupervisorCacheOperationRange(IServiceProvider serviceProvider) : base (serviceProvider) 
        {
            this.Supervisor = serviceProvider.GetRequiredService<ISupervisorFactoryOperationRange>().CreateSupervisor(0);          
        }
        #endregion

        #region Methods
        public async Task Initialize()
        {
            IEnumerable<OperationRange> operationRanges = await this.Supervisor.GetOperationRanges();
            foreach (var item in operationRanges)
            {
                await this.CacheOperationRangeService.Set(item.Id, item);
            }
        }

        public async Task<IEnumerable<OperationRange>> GetOperationRanges()
        {
            return await this.Supervisor.GetOperationRanges();
        }

        public async Task<OperationRange> GetOperationRange(string id)
        {
            OperationRange operationRange = await this.CacheOperationRangeService.Get((arg) => arg.Id == id);
            if (operationRange != null)
            {
                await this.SetOperationRangeDetails(operationRange);
            }
            return operationRange;
        }
        public async Task<IEnumerable<OperationRange>> GetOperationRanges(string programId)
        {
            List<OperationRange> operationRanges = (await this.CacheOperationRangeService.GetAll((arg) => arg.ProgramId == programId)).ToList();
            if (operationRanges != null)
            {
                operationRanges.ForEach(async (arg) =>
                {
                    await this.SetOperationRangeDetails(arg);
                });
            }
            return operationRanges;
        }
        public async Task<ResultCode> AddOperationRange(OperationRange operationRange)
        {
            ResultCode code = await this.Supervisor.AddOperationRange(operationRange);
            if (code == ResultCode.Ok)
            {
                if (operationRange.Condition != null)
                {
                    await this.CacheConditionService.Set(operationRange.Condition.Id, operationRange.Condition);
                }
                await this.CacheOperationRangeService.Set(operationRange.Id, operationRange);
            }
            return code;
        }
        public async Task<ResultCode> DeleteOperationRange(OperationRange operationRange)
        {
            ResultCode code = await this.Supervisor?.DeleteOperationRange(operationRange);
            if (code == ResultCode.Ok) 
            {
                if (operationRange.ConditionId != null)
                {
                    await this.CacheConditionService.Delete(operationRange.ConditionId);
                }
                await this.CacheOperationRangeService.Delete(operationRange.Id);
            }
            return code;
        }
        public async Task<ResultCode> DeleteOperationRanges(IEnumerable<OperationRange> operationRanges)
        {
            ResultCode code = await this.Supervisor?.DeleteOperationRanges(operationRanges);
            if (code == ResultCode.Ok) 
            {
                foreach (OperationRange operationRange in operationRanges) 
                {
                    if (operationRange.ConditionId != null)
                    {
                        await this.CacheConditionService.Delete(operationRange.ConditionId);
                    }

                    await this.CacheOperationRangeService.Delete(operationRange.Id);
                }
            }
            return code;
        }
        private async Task SetOperationRangeDetails(OperationRange operationRange)
        {
            operationRange.Condition = await this.CacheConditionService.Get((arg) => arg.Id == operationRange.ConditionId);
        }
        #endregion
    }
}
