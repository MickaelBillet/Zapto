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
            Log.Information("EventHandlerSensorData.Handle");
            try
            {
                await this.ApplicationSensorServices.ReadData(message);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in EventHandlerSensorData.Handle");
            }
        }
    }
}
