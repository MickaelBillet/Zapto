using Framework.Core.Base;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using WeatherZapto.Data;

namespace WeatherZapto.WebServer.Services
{
    public class CallOpenWeather : IHealthCheck
    {
        private const int WarningLimit = 8000000;
        private const int ErrorLimit = 1000000;

        #region Properties
        private ISupervisorCall SupervisorCall { get; }
        #endregion

        #region Constructor
        public CallOpenWeather(ISupervisorCall supervisorCall)
        {
            this.SupervisorCall = supervisorCall;
        }
        #endregion

        #region Methods
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
                                                                CancellationToken cancellationToken = default(CancellationToken))
        {
            long? dayCount = await this.SupervisorCall.GetDayCallsCount(new DateOnly(Clock.Now.Year, Clock.Now.Month, Clock.Now.Day));
            long? monthCount = await this.SupervisorCall.GetLast30DaysCallsCount();

            if ((monthCount > WarningLimit) && (monthCount < ErrorLimit))
            {
                return (HealthCheckResult.Degraded($"Calls within the month : {monthCount} - within the day : {dayCount}"));
            }
            else if (monthCount >= ErrorLimit) 
            {
                return (HealthCheckResult.Unhealthy($"Calls within the month : {monthCount} - within the day : {dayCount}"));
            }

            return (HealthCheckResult.Healthy($"Calls within the month : {monthCount} - within the day : {dayCount}"));
        }
        #endregion
    }
}
