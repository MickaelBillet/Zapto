using System.Net.Http;
using System.Threading.Tasks;

namespace Framework.Infrastructure.Services
{
    public interface IErrorHandlerWebService
	{
		Task<bool> HandleErrorAsync(HttpResponseMessage message, string obj, int attempt);
		void HandleError(string msg);
		void HandleInternetError();
	}

	public class WebServiceMessage
	{
		public string Error { get; set; }
	}
}
