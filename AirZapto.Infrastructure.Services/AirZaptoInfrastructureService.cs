using AirZapto.Application.Infrastructure;
using AirZapto.Infrastructure.WebServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AirZapto.Infrastructure.Services
{
    public static class AirZaptoInfrastructureService
    {
        public static void AddAirZaptoWebServices(this IServiceCollection services,
                                                        IConfiguration configuration,
                                                        string httpClientName)
        {
            services.AddTransient<ISensorService, SensorService>((service) =>
            {
                return new SensorService(service, configuration, httpClientName);
            });
            services.AddTransient<ISensorDataService, SensorDataService>((service) =>
            {
                return new SensorDataService(service, configuration, httpClientName);
            });
            services.AddTransient<IHealthCheckAirZaptoService, HealthCheckAirZaptoService>((service) =>
            {
                return new HealthCheckAirZaptoService(service, configuration, httpClientName);
            });
        }
    }
}
