using Connect.Model;
using Framework.Core.Base;
using Microsoft.Extensions.DependencyInjection;

namespace Connect.Data.Supervisors
{
    public sealed class SupervisorCacheCondition : SupervisorCache
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
        public async Task<Condition> GetCondition(string id)
        {
            Condition condition = await this.CacheConditionService.Get((arg) => arg.Id == id);
            return condition;
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
