using Connect.Data;
using Connect.Model;
using Framework.Core.Base;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Serilog;

namespace Connect.WebServer.Services
{
    public class SensorHealthCheck : IHealthCheck
    {
		#region Properties
		private ISupervisorSensor Supervisor { get; }
		#endregion

		#region Constructor
		public SensorHealthCheck(IServiceProvider serviceProvider, IConfiguration configuration)
		{
			this.Supervisor = serviceProvider.GetRequiredService<ISupervisorFactorySensor>().CreateSupervisor(int.Parse(configuration["Cache"]!));
		}
		#endregion

		#region Methods
		public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
																CancellationToken cancellationToken = default(CancellationToken))
        {
			try
			{
				IEnumerable<Sensor> sensors = (await this.Supervisor.GetSensors()).Where(sensor => sensor.Type != DeviceType.Sensor_Water_Leak);
				string description = string.Empty;
				int sensorsDownCount = 0;
				int sensorsUpCount = 0;

				foreach (Sensor sensor in sensors)
				{
					if (sensor.IsRunning == RunningStatus.UnHealthy)
					{
						description = $"{description} {sensor.Name}:{sensor.Channel}";
						sensorsDownCount++;
					}
					else if (sensor.IsRunning == RunningStatus.Healthy)
					{
						sensorsUpCount++;
					}
				}

				if (sensors.Count() == sensorsDownCount)
				{
					return HealthCheckResult.Unhealthy("All the sensors are down");
				}
				else if (sensors.Count() == sensorsUpCount)
				{
					return HealthCheckResult.Healthy("All the sensors are up");
				}
				else
				{
					return HealthCheckResult.Degraded(description);
				}
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Error in SensorHealthCheck.CheckHealthAsync");
            }
			return HealthCheckResult.Degraded();
        }
		#endregion
	}
}
