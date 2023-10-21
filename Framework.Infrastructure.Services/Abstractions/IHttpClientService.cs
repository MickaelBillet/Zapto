using System.Net.Http;

namespace Framework.Infrastructure.Services
{
    public interface IHttpClientService
	{
		public HttpClient GetClient(string name);
	}
}
