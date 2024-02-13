using System.Threading.Tasks;

namespace Framework.Infrastructure.Services
{
    public interface IAlertService
    {
        Task SendAlertAsync(string locationId, string title, string body);
    }
}
