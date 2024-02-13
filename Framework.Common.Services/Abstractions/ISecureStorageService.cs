using System.Threading.Tasks;

namespace Framework.Infrastructure.Services
{
    public interface ISecureStorageService
	{
		public Task SetAsync(string key, string value);
		public Task<string> GetAsync(string key);
		public void RemoveAll();
	}
}
