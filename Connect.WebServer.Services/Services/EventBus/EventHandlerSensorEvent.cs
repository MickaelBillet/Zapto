using Connect.Application;
using Connect.Model;
using InMemoryEventBus.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Connect.WebServer.Services
{
    public class EventHandlerSensorEvent : IEventHandler<SensorEvent>
    {
        private IApplicationSensorServices ApplicationSensorServices { get; }

        public EventHandlerSensorEvent(IServiceProvider serviceProvider) 
        {
            this.ApplicationSensorServices = serviceProvider.GetRequiredService<IApplicationSensorServices>();
        }

        public async ValueTask Handle(SensorEvent? message, CancellationToken cancellationToken = default)
        {
            Log.Information("EventHandlerSensorEvent.Handle");
            try
            {
                await this.ApplicationSensorServices.ReadEvent(message);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in EventHandlerSensorEvent.Handle");
            }
        }
    }
}
