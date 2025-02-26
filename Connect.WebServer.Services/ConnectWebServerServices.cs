﻿using Connect.Model;
using Framework.Common.Services;
using Framework.Core.InMemoryEventBus.Registration;
using Framework.Core.Model;
using Framework.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Connect.WebServer.Services
{
    public static class ConnectWebServerServices
    {
        public static void AddConnectServices(this IServiceCollection services, string communicationType)
        {
            services.AddScoped<ISendCommandService, SendCommandService>();   
            services.AddScoped<ISendMessageToArduinoService, SendMessageToArduinoService>((provider) => new SendMessageToArduinoService(provider, communicationType));
        }

        public static void AddInMemoryEventServices(this IServiceCollection services)
        {
            services.AddInMemoryEvent<CommandStatus, EventHandlerCommandStatus>();
            services.AddInMemoryEvent<SensorData, EventHandlerSensorData>();
            services.AddInMemoryEvent<SystemStatus, EventHandlerServerStatus>();
            services.AddInMemoryEvent<SensorEvent, EventHandlerSensorEvent>();
            services.AddTransient<IEventBusProducerConnect, EventBusProducerConnect>();
        }
    }
}
