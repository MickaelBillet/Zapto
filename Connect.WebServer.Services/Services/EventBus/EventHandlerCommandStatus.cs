using Connect.Application;
using Connect.Model;
using InMemoryEventBus.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Connect.WebServer.Services
{
    public class EventHandlerCommandStatus : IEventHandler<CommandStatus>
    {
        private IApplicationPlugServices ApplicationPlugServices  { get; }

        public EventHandlerCommandStatus(IServiceProvider serviceProvider) 
        {
            this.ApplicationPlugServices = serviceProvider.GetRequiredService<IApplicationPlugServices>();
        }

        public async ValueTask Handle(CommandStatus? message, CancellationToken cancellationToken = default)
        {
            Log.Information("EventHandlerCommandStatus.Handle");
            await this.ApplicationPlugServices.ReadStatus(message);
        }
    }
}
