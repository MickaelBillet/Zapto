using Microsoft.Extensions.DependencyInjection;
using WeatherZapto.Data.Supervisors;

namespace WeatherZapto.Data
{
    public static class WeatherZaptoService
    {
        public static void AddSupervisor(this IServiceCollection services)
        {
            services.AddScoped<ISupervisorLogs, SupervisorLogs>();
            services.AddScoped<ISupervisorVersion, SupervisorVersion>();
            services.AddScoped<ISupervisorAirPollution, SupervisorAirPollution>();
            services.AddScoped<ISupervisorWeather, SupervisorWeather>();
            services.AddScoped<ISupervisorCall, SupervisorCall>();   
            services.AddScoped<ISupervisorTemperature, SupervisorTemperature>();
        }
    }
}
