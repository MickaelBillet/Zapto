using Framework.Core.Base;
using System.Threading.Tasks;

namespace Framework.Infrastructure.Services
{
    public interface IEventBusProducerConnect
    {
        Task Publish(MessageArduino message);
    }
}