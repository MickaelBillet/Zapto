using Connect.Data.Entities;
using Connect.Data.Mappers;
using Connect.Data.Services.Repositories;
using Connect.Model;
using Framework.Core.Base;
using Framework.Data.Abstractions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Connect.Data.Supervisors
{
    public sealed class SupervisorCondition : ISupervisorCondition
    {
        private readonly Lazy<IRepository<ConditionEntity>> _lazyConditionRepository;

        #region Properties
        private IRepository<ConditionEntity> ConditionRepository => _lazyConditionRepository.Value;
        #endregion

        #region Constructor
        public SupervisorCondition(IDataContextFactory dataContextFactory, IRepositoryFactory repositoryFactory, IConfiguration configuration)
        {
            ConnectionType type = new ConnectionType()
            {
                ConnectionString = configuration["ConnectionStrings:DefaultConnection"],
                ServerType = ConnectionType.GetServerType(configuration["ConnectionStrings:ServerType"]),
            };

            IDataContext? context = dataContextFactory.CreateDbContext(type.ConnectionString, type.ServerType)?.context;
            if (context != null)
            {
                _lazyConditionRepository = repositoryFactory.CreateRepository<ConditionEntity>(context);
            }
        }
        #endregion

        #region Methods
        public async Task<ResultCode> ConditionExists(string id)
        {
            return (await this.ConditionRepository.GetAsync(id) != null) ? ResultCode.Ok : ResultCode.ItemNotFound;
        }

        public async Task<IEnumerable<Condition>> GetConditions()
        {
            IEnumerable<ConditionEntity> entities = await this.ConditionRepository.GetCollectionAsync();
            return entities.Select(item => ConditionMapper.Map(item));
        }

        public async Task<Condition> GetCondition(string id)
        {
            ConditionEntity entity = await this.ConditionRepository.GetAsync(id);
            return ConditionMapper.Map(entity);
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
