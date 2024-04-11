using Connect.Data;
using Framework.Core.Domain;
using Microsoft.Extensions.Diagnostics.HealthChecks;

#nullable disable

namespace Connect.WebServer.Services
{
    public class ErrorHealthCheck : IHealthCheck
    {
		#region Properties
		private ISupervisorLog Supervisor { get; }
		#endregion

		#region Constructor
		public ErrorHealthCheck(ISupervisorFactoryLog factory)
		{
			this.Supervisor = factory.CreateSupervisor();
		}
		#endregion

		#region Methods
		public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
														CancellationToken cancellationToken = default(CancellationToken))
        {
			IEnumerable<Logs> logs = await this.Supervisor.GetLogsInf24H();
			int countError = logs.Where<Logs>(log => log.Level.Contains("Error")).Count<Logs>();
			int countFatal = logs.Where<Logs>(log => log.Level.Contains("Fatal")).Count<Logs>();
            if (countError + countFatal > 0)
			{
				return (HealthCheckResult.Degraded($"Count Error : {countError} - Count Fatal : {countFatal}"));
			}
            return (HealthCheckResult.Healthy("No Error"));
        }
		#endregion
	}
}
