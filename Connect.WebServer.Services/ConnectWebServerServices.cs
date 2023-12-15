using Microsoft.Extensions.DependencyInjection;

namespace Connect.WebServer.Services
{
    public static class ConnectWebServerServices
    {
        public static void AddConnectWebServices(this IServiceCollection services)
        {
            services.AddScoped<ISendCommandService, SendCommandService>();            
        }
    }
}
