using System.Threading.Tasks;

namespace Connect.Application.Infrastructure
{
    public interface ISensorService
    {
        Task<bool?> Leak(string? sensorId, string leak);
    }
}
