using AirZapto.Data;
using Framework.Core.Base;
using Framework.Core.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace AirZapto.WebServer.Services
{
    public class ErrorHealthCheck : IHealthCheck
    {
		#region Properties
		private ISupervisorLogs Supervisor { get; }

		#endregion
		#region Constructor
		public ErrorHealthCheck(IServiceProvider serviceProvider, IConfiguration configuration)
		{
            this.Supervisor = serviceProvider.GetRequiredService<ISupervisorLogs>();
        }

        #endregion

        #region Methods
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
														CancellationToken cancellationToken = default(CancellationToken))
        {
			(ResultCode code, IEnumerable<Logs>? logs) = await this.Supervisor.GetLogsInf24HAsync();
            if ((code == ResultCode.Ok) && (logs != null)) 
            {
                int countError = logs.Where<Logs>(log => log.Level.Contains("Error")).Count<Logs>();
                int countFatal = logs.Where<Logs>(log => log.Level.Contains("Fatal")).Count<Logs>();
                if (countError + countFatal > 0)
                {
                    return (HealthCheckResult.Degraded($"Count Error : {countError} - Count Fatal : {countFatal}"));
                }
            }
            else
            {
                return (HealthCheckResult.Degraded("Error Db"));
            }

            return (HealthCheckResult.Healthy("No Error"));
        }
		#endregion
	}
}
