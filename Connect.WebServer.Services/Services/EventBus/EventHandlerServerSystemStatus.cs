using Connect.Application;
using Framework.Core.Model;
using InMemoryEventBus.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Connect.WebServer.Services
{
    public class EventHandlerServerStatus : IEventHandler<SystemStatus>
    {
        private IApplicationServerIotServices ApplicationServerIotServices  { get; }

        public EventHandlerServerStatus(IServiceProvider serviceProvider) 
        {
            this.ApplicationServerIotServices = serviceProvider.GetRequiredService<IApplicationServerIotServices>();
        }

        public async ValueTask Handle(SystemStatus? message, CancellationToken cancellationToken = default)
        {
            Log.Information("EventHandlerServerStatus.Handle");
            await this.ApplicationServerIotServices.ReadStatus(message);
        }
    }
}
