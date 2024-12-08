using Connect.Data.Entities;
using Connect.Data.Mappers;
using Connect.Data.Services.Repositories;
using Connect.Model;
using Framework.Core.Base;
using Framework.Data.Abstractions;

namespace Connect.Data.Supervisors
{
    public sealed class SupervisorCondition : Supervisor, ISupervisorCondition
    {
        private readonly Lazy<IRepository<ConditionEntity>> _lazyConditionRepository;

        #region Properties
        private IRepository<ConditionEntity> ConditionRepository => _lazyConditionRepository.Value;
        #endregion

        #region Constructor
        public SupervisorCondition(IDalSession session, IRepositoryFactory repositoryFactory)
        {
            _lazyConditionRepository = repositoryFactory.CreateRepository<ConditionEntity>(session);
        }
        #endregion

        #region Methods
        public async Task<ResultCode> ConditionExists(string id)
        {
            return (await this.ConditionRepository.GetAsync(id) != null) ? ResultCode.Ok : ResultCode.ItemNotFound;
        }

        public async Task<Condition> GetCondition(string id)
        {
            ConditionEntity entity = await this.ConditionRepository.GetAsync(id);
            return ConditionMapper.Map(entity);
        }

        public async Task<IEnumerable<Condition>> GetConditions()
        {
            return (await this.ConditionRepository.GetCollectionAsync()).Select((arg) => ConditionMapper.Map(arg));
        }

        public async Task<ResultCode> AddCondition(Condition condition)
        {
            condition.Id = string.IsNullOrEmpty(condition.Id) ? Guid.NewGuid().ToString() : condition.Id;
            int res = await this.ConditionRepository.InsertAsync(ConditionMapper.Map(condition));
            ResultCode result = (res > 0) ? ResultCode.Ok : ResultCode.CouldNotCreateItem;
            return result;
        }

        public async Task<ResultCode> UpdateCondition(string id, Condition condition)
        {
            ResultCode result = await this.ConditionExists(id);

            if (result == ResultCode.Ok)
            {
                int res = await this.ConditionRepository.UpdateAsync(ConditionMapper.Map(condition));
                result = (res > 0) ? ResultCode.Ok : ResultCode.CouldNotUpdateItem;
            }

            return result;
        }

        public async Task<ResultCode> DeleteCondition(Condition condition)
        {
            return (await this.ConditionRepository.DeleteAsync(ConditionMapper.Map(condition)) > 0) ? ResultCode.Ok : ResultCode.CouldNotDeleteItem;
        }

        #endregion
    }
}
