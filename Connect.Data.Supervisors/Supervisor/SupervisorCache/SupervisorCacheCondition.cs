using Connect.Model;
using Framework.Core.Base;
using Microsoft.Extensions.DependencyInjection;

namespace Connect.Data.Supervisors
{
    public sealed class SupervisorCacheCondition : SupervisorCache, ISupervisorCondition
    {
        #region Services
        private ISupervisorCondition Supervisor { get; }
        #endregion

        #region Constructor
        public SupervisorCacheCondition(IServiceProvider serviceProvider) : base (serviceProvider) 
        {
            this.Supervisor = serviceProvider.GetRequiredService<ISupervisorCondition>();
        }
        #endregion

        #region Methods
        public async Task Initialize()
        {
            IEnumerable<Condition> conditions = await this.Supervisor.GetConditions();
            foreach (var item in conditions)
            {
                await this.CacheConditionService.Set(item.Id, item);
            }
        }
        public Task<IEnumerable<Condition>> GetConditions()
        {
            return this.Supervisor.GetConditions();
        }
        public async Task<Condition> GetCondition(string id)
        {
            return await this.CacheConditionService.Get((arg) => arg.Id == id);
        }
        public async Task<ResultCode> AddCondition(Condition condition)
        {
            ResultCode code = await this.Supervisor.AddCondition(condition);
            if (code == ResultCode.Ok)
            {
                await this.CacheConditionService.Set(condition.Id, condition);  
            }
            return code;
        }
        public async Task<ResultCode> UpdateCondition(string id, Condition condition)
        {
            ResultCode code = await this.Supervisor.UpdateCondition(id, condition);
            if (code == ResultCode.Ok)
            {
                await this.CacheConditionService.Set(condition.Id, condition);
            }
            return code;
        }
        public async Task<ResultCode> DeleteCondition(Condition condition)
        {
            ResultCode code = await this.Supervisor.DeleteCondition(condition);
            if (code == ResultCode.Ok)
            {
                await this.CacheConditionService.Delete(condition.Id);
            }
            return code;
        }
        #endregion
    }
}
