using AirZapto.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Authentication.Application
{
    public static class AuthenticationAppService
	{
		public static void AddApplicationAirZaptoServices(this IServiceCollection services)
		{
			services.AddTransient<IApplicationOpenIdConfigurationServicecs, ApplicationOpenIdConfigurationService>();
        }   
    }
}
