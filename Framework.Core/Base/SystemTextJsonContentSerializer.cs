using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Framework.Core.Base
{
	public sealed class SystemTextJsonContentSerializer
    {
        public SystemTextJsonContentSerializer()
        {
        }

        public async Task<T> DeserializeAsync<T>(HttpContent content, JsonSerializerOptions options)
        {
            using var utf8Json = await content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<T>(utf8Json, options);
        }

        public async Task<T> DeserializeAsync<T>(Stream stream, JsonSerializerOptions options)
        {
            return await JsonSerializer.DeserializeAsync<T>(stream, options);
        }

        public async Task<string> SerializeAsync<T>(T item, JsonSerializerOptions options)
        {
            return await Task.Run<string>(() =>
            {
                return JsonSerializer.Serialize<T>(item, options);           
            });
        }

        public async Task<MemoryStream> SerializeWithStreamAsync<T>(T item)
        {
            MemoryStream stream = new MemoryStream();
            await JsonSerializer.SerializeAsync(stream, item);
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }
    }
}
