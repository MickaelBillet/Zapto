using Authentication.Application.Infrastructure;
using Authentication.Infrastructure.WebServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Authentication.Infrastructure.Services
{
    public static class AuthenticationInfrastructureServices
    {
        public static void AddOpenWeatherWebServices(this IServiceCollection services,
                                                            IConfiguration configuration,
                                                            string httpClientName)
        {
            services.AddTransient<IOpenIdConfigurationService, OpenIdConfigurationService>((service) =>
            {
                return new OpenIdConfigurationService(service, configuration, httpClientName);
            });
        }
    }
}
