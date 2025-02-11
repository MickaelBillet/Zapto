﻿using Framework.Infrastructure.Services;
using WebApplicationWebSocket.Services;

namespace WebApplicationWebSocket.Configuration
{
    public static class ServiceConfiguration
    {
        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<WebSocketService>();
            services.AddTransient<IWSMessageManager, WSMessageManager>();
        }
    }
}
