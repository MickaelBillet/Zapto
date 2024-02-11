using Connect.Application.Infrastructure;
using Connect.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Connect.Infrastructure.WebServices
{
    public class ConditionService : ConnectWebService, IConditionService
    {
        #region Property

        #endregion

        #region Constructor

        public ConditionService(IServiceProvider serviceProvider, string httpClientName) : base(serviceProvider, httpClientName)
        {
        }

        #endregion

        #region Method

        public async Task<bool?> DeleteConditionAsync(Condition condition, CancellationToken token = default)
        {
            return await WebService.DeleteAsync<Condition>(ConnectConstants.RestUrlConditionsId, condition.Id, token); ;
        }

        public async Task<bool?> AddConditionAsync(Condition condition, CancellationToken token = default)
        {
            return await WebService.PostAsync<Condition>(ConnectConstants.RestUrlConditions, condition, token); ;
        }

        public async Task<bool?> UpdateConditionAsync(Condition condition, CancellationToken token = default)
        {
            return await WebService.PutAsync<Condition>(ConnectConstants.RestUrlConditionsId, condition, condition.Id, token); ;
        }

        #endregion
    }
}
