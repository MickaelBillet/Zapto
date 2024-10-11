using Framework.Core.Base;
using System.Threading.Tasks;

namespace Framework.Infrastructure.Services
{
    public interface IInMemoryEvent
    {
        Task Publish(MessageArduino message);
    }
}