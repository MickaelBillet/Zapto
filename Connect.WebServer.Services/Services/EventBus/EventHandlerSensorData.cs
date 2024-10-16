using Connect.Application;
using Connect.Model;
using InMemoryEventBus.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Connect.WebServer.Services
{
    public class EventHandlerSensorData : IEventHandler<SensorData>
    {
        private IApplicationSensorServices ApplicationSensorServices { get; }

        public EventHandlerSensorData(IServiceProvider serviceProvider) 
        {
            this.ApplicationSensorServices = serviceProvider.GetRequiredService<IApplicationSensorServices>();
        }

        public async ValueTask Handle(SensorData? message, CancellationToken cancellationToken = default)
        {
            Log.Information("ApplicationSensorServices.Handle");
            await this.ApplicationSensorServices.ReadData(message);
        }
    }
}
