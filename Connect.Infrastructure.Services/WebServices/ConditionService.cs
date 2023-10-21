using Connect.Application.Infrastructure;
using Connect.Model;
using Framework.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Serilog;
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

        public ConditionService(IServiceProvider serviceProvider, IConfiguration configuration, string httpClientName) : base(serviceProvider, configuration, httpClientName)
        {
        }

        #endregion

        #region Method

        public async Task<bool?> DeleteConditionAsync(Condition condition, CancellationToken token = default)
        {
            bool? res = false;

            try
            {
                res = await WebService.DeleteAsync<Condition>(ConnectConstants.RestUrlConditionsId, condition.Id, token);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

            return res;
        }

        public async Task<bool?> AddConditionAsync(Condition condition, CancellationToken token = default)
        {
            bool? res = false;

            try
            {
                res = await WebService.PostAsync<Condition>(ConnectConstants.RestUrlConditions, condition, token);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

            return res;
        }

        public async Task<bool?> UpdateConditionAsync(Condition condition, CancellationToken token = default)
        {
            bool? res = false;

            try
            {
                res = await WebService.PutAsync<Condition>(ConnectConstants.RestUrlConditionsId, condition, condition.Id, token);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

            return res;
        }

        #endregion
    }
}
