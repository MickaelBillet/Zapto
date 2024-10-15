using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Infrastructure.Services
{
    public interface IWebService
    {
        Task<IEnumerable<T>?> GetCollectionAsync<T>(string url, JsonSerializerOptions options, CancellationToken cancellationToken = default, int attempt = 1);
        Task<T> GetAsync<T>(string url, string? id, JsonSerializerOptions? options, CancellationToken cancellationToken = default, int attempt = 1) where T : new();
        Task<bool?> PostAsync<T>(string url, T item, CancellationToken cancellationToken = default, int attempt = 1);
        Task<bool?> PutAsync<T>(string url, T item, string? id, CancellationToken cancellationToken = default, int attempt = 1);
        Task<bool?> DeleteAsync<T>(string url, string? id, CancellationToken cancellationToken = default, int attempt = 1);
        Task<bool?> HeadAsync<T>(string url, string? id, CancellationToken cancellationToken = default, int attempt = 1);
        Task<Stream?> GetImageStreamAsync(string url);

	}
}
