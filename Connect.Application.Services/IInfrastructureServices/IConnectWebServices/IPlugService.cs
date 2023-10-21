using Connect.Model;
using System.Threading.Tasks;

namespace Connect.Application.Infrastructure
{
    public interface IPlugService
    {
        Task<bool?> ManualProgrammingAsync(Plug plug);

        Task<bool?> OnOffAsync(Plug plug);
    }
}
