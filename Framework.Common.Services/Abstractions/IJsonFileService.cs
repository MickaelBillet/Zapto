using System.Threading.Tasks;

namespace Framework.Infrastructure.Services
{
    public interface IJsonFileService
    {
        public Task WriteFile<T>(T item) where T : class;

        public Task<T> ReadFile<T>() where T : class;
    }
}
