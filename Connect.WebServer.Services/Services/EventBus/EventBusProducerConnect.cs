using Connect.Model;
using Framework.Core.Base;
using Framework.Core.Model;
using Framework.Infrastructure.Services;
using InMemoryEventBus.Contracts;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace Connect.WebServer.Services
{
    public class EventBusProducerConnect : IEventBusProducerConnect
    {
        #region Properties
        private IServiceProvider ServiceProvider { get; }
        #endregion

        #region Constructor
        public EventBusProducerConnect(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }
        #endregion

        #region Methods
        public async Task Publish(MessageArduino message)
        {
            if (message.Header == ConnectConstants.PlugStatus)
            {
                await this.PublishEvent<PlugStatus>(message.Payload);
            }
            else if (message.Header == ConnectConstants.SensorData)
            {
                await this.PublishEvent<SensorData>(message.Payload);
            }
            else if (message.Header == ConnectConstants.SensorEvent)
            {
                await this.PublishEvent<SensorEvent>(message.Payload);
            }
            else if (message.Header == ConnectConstants.ServerIotStatus)
            {
                await this.PublishEvent<SystemStatus>(message.Payload);
            }
        }

        private async Task PublishEvent<T>(string payload)
        {
            T? item = JsonSerializer.Deserialize<T>(payload);
            IProducer<T> producer = this.ServiceProvider.GetRequiredService<IProducer<T>>();
            Func<Task>? publishEventsFn = async () =>
            {
                var @event = new Event<T>(item);
                await producer.Publish(@event).ConfigureAwait(false);
            };

            if (publishEventsFn != null)
            {
                await publishEventsFn.Invoke();
            }
        }
        #endregion
    }
}
