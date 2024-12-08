using Connect.Application.Infrastructure;
using Connect.Model;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Connect.Application.Services
{
    internal sealed class ApplicationConditionServices : IApplicationConditionServices
	{
        #region Services
        private IConditionService? ConditionService { get; }
		#endregion

		#region Constructor
		public ApplicationConditionServices(IServiceProvider serviceProvider)
		{
			this.ConditionService = serviceProvider.GetService<IConditionService>();
		}
		#endregion

		#region Methods
		public async Task<bool?> UpdateCondition(Condition condition)
		{
			return (this.ConditionService != null) ? await this.ConditionService.UpdateConditionAsync(condition) : null;
		}
		#endregion
	}
}
