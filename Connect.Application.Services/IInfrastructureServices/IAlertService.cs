using System.Threading.Tasks;

namespace Connect.Application.Infrastructure
{
    public interface IAlertService
    {
        Task SendAlertAsync(string locationId, string title, string body);
    }
}
