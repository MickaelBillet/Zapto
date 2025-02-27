using Connect.Data;
using Framework.Core.Domain;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Serilog;

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
			try
			{
				IEnumerable<Logs> logs = await this.Supervisor.GetLogsInf24H();
				int countError = logs.Where<Logs>(log => log.Level.Contains("Error")).Count<Logs>();
				int countFatal = logs.Where<Logs>(log => log.Level.Contains("Fatal")).Count<Logs>();
				if (countError + countFatal > 0)
				{
					return (HealthCheckResult.Degraded($"Count Error : {countError} - Count Fatal : {countFatal}"));
				}
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Error in ErrorHealthCheck.CheckHealthAsync");
            }
            return (HealthCheckResult.Healthy("No Error"));
        }
		#endregion
	}
}
