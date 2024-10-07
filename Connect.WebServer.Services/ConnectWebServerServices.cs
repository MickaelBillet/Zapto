using AirZapto.WebServer.Services;
using Framework.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Connect.WebServer.Services
{
    public static class ConnectWebServerServices
    {
        public static void AddConnectWebServices(this IServiceCollection services)
        {
            services.AddScoped<ISendCommandService, SendCommandService>();   
            services.AddSingleton<IWSMessageManager, WebSocketMessageManager>((provider) => new WebSocketMessageManager(new WebSocketService(), provider));
        }
    }
}
