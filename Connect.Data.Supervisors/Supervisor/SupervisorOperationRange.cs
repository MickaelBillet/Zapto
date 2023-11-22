using Connect.Data.Entities;
using Connect.Data.Mappers;
using Connect.Data.Services.Repositories;
using Connect.Model;
using Framework.Core.Base;
using Framework.Data.Abstractions;

namespace Connect.Data.Supervisors
{
    public sealed class SupervisorOperationRange : ISupervisorOperationRange
    {
        private readonly Lazy<IRepository<OperationRangeEntity>> _lazyOperationRangeRepository;
        private readonly Lazy<IRepository<ConditionEntity>> _lazyConditionRepository;

        #region Properties
        private IRepository<OperationRangeEntity> OperationRangeRepository => _lazyOperationRangeRepository.Value;
        private IRepository<ConditionEntity> ConditionRepository => _lazyConditionRepository.Value;
        #endregion

        #region Constructor
        public SupervisorOperationRange(IDalSession session, IRepositoryFactory repositoryFactory)
        {
            _lazyOperationRangeRepository = repositoryFactory.CreateRepository<OperationRangeEntity>(session);
            _lazyConditionRepository = repositoryFactory?.CreateRepository<ConditionEntity>(session);
        }
        #endregion

        #region Methods

        public async Task<ResultCode> OperationRangeExists(string id)
        {
            return (await this.OperationRangeRepository.GetAsync(id) != null) ? ResultCode.Ok : ResultCode.ItemNotFound;
        }

        /// <summary>
        /// Get operationRanges with conditions
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<OperationRange>> GetOperationRanges()
        {
            IEnumerable<OperationRange> operationRanges = null;
            IEnumerable<OperationRangeEntity> entities = await this.OperationRangeRepository.GetCollectionAsync();
            if (entities != null)
            {
                operationRanges = entities.Select(item => OperationRangeMapper.Map(item));
                foreach (OperationRange operationRange in operationRanges)
                {
                    operationRange.Condition = ConditionMapper.Map(await this.ConditionRepository.GetAsync((arg) => arg.Id == operationRange.ConditionId));
                }
            }

            return operationRanges;
        }

        /// <summary>
        /// Get operationRange with condition
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<OperationRange> GetOperationRange(string id)
        {
            OperationRange operationRange = null;
            OperationRangeEntity entity = await this.OperationRangeRepository.GetAsync(id);
            if (entity != null)
            {
                operationRange = OperationRangeMapper.Map(entity);
                operationRange.Condition = ConditionMapper.Map(await this.ConditionRepository.GetAsync((arg) => arg.Id == operationRange.ConditionId));
            }

            return operationRange;
        }

        public async Task<IEnumerable<OperationRange>> GetOperationRanges(string programId)
        {
            //Read the operation ranges of the program
            List<OperationRange> operationRanges = null;
            IEnumerable<OperationRangeEntity> entities = await this.OperationRangeRepository.GetCollectionAsync((arg) => arg.ProgramId == programId);
            if (entities != null)
            {
                operationRanges = entities.Select(item => OperationRangeMapper.Map(item)).ToList();
                foreach (OperationRange operationRange in operationRanges)
                {
                    operationRange.Condition = ConditionMapper.Map(await this.ConditionRepository.GetAsync((arg) => arg.Id == operationRange.ConditionId));
                }
            }

            return operationRanges;
        }

        public async Task<ResultCode> AddOperationrange(OperationRange operationRange)
        {
            ResultCode result = ResultCode.CouldNotCreateItem;
            if (operationRange.Condition != null)
            {
                operationRange.Condition.Id = string.IsNullOrEmpty(operationRange.Condition.Id) ? Guid.NewGuid().ToString() : operationRange.Condition.Id;
                await this.ConditionRepository.InsertAsync(ConditionMapper.Map(operationRange.Condition));
            }

            operationRange.Id = string.IsNullOrEmpty(operationRange.Id) ? Guid.NewGuid().ToString() : operationRange.Id;
            int res = await this.OperationRangeRepository.InsertAsync(OperationRangeMapper.Map(operationRange));
            if (res > 0)
            {
                result = ResultCode.Ok;
            }
            else
            {
                await this.ConditionRepository.DeleteAsync(ConditionMapper.Map(operationRange.Condition));
                result = ResultCode.CouldNotCreateItem;
            }


            return result;
        }

        public async Task<ResultCode> DeleteOperationRange(OperationRange operationRange)
        {
            if (operationRange?.ConditionId != null)
            {
                ConditionEntity conditionEntity = await this.ConditionRepository.GetAsync(operationRange.ConditionId);
                if (conditionEntity != null)
                {
                    await this.ConditionRepository.DeleteAsync(conditionEntity);
                }
            }

            return (await this.OperationRangeRepository.DeleteAsync(OperationRangeMapper.Map(operationRange)) > 0) ? ResultCode.Ok : ResultCode.CouldNotDeleteItem;
        }

        public async Task<ResultCode> DeleteOperationRanges(IEnumerable<OperationRange> operationRanges)
        {
            ResultCode result = ResultCode.CouldNotDeleteItem;
            foreach (OperationRange operationRange in operationRanges)
            {
                if (operationRange?.ConditionId != null)
                {
                    ConditionEntity conditionEntity = await this.ConditionRepository.GetAsync(operationRange.ConditionId);
                    if (conditionEntity != null)
                    {
                        await this.ConditionRepository.DeleteAsync(conditionEntity);
                    }
                }
            }

            if (operationRanges.Count() == 0)
            {
                return ResultCode.Ok;
            }
            else
            {
                int count = operationRanges.Count();
                foreach (OperationRange operationRange in operationRanges)
                {
                    if (await this.OperationRangeRepository.DeleteAsync(OperationRangeMapper.Map(operationRange)) > 0)
                    {
                        count--;
                    }
                }

                if (count == 0)
                {
                    return ResultCode.Ok;
                }
            }

            return result;
        }

        #endregion
    }
}
