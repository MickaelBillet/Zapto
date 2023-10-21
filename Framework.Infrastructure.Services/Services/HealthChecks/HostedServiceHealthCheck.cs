using Framework.Core.Base;
using Framework.Core.Model;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Infrastructure.Services
{
    public class HostedServiceHealthCheck : IHealthCheck
    {
		#region Properties
		private SystemStatus Status { get; set; } = null;
		#endregion
		public string Name => "service_check";

		#region Constructor

		public HostedServiceHealthCheck()
		{

		}

		#endregion

		#region Methods
		public void SetStatus(SystemStatus status )
		{
			this.Status = status;
		}

		public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
														CancellationToken cancellationToken = default(CancellationToken))
        {
            if (this.Status != null)
            {
				if (this.Status.Date + new TimeSpan(0, 10, 0) > Clock.Now)
				{
					return Task.FromResult(HealthCheckResult.Healthy("The service Arduino is started"));
				}
            }

            return Task.FromResult(HealthCheckResult.Unhealthy("The service is down"));
        }
		#endregion
	}
}
