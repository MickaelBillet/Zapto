using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Framework.Infrastructure.Services
{
    public class ErrorHandlerWebService : IErrorHandlerWebService
	{
		#region Property

		#endregion

		#region Constructor

		public ErrorHandlerWebService(IServiceProvider provider)
		{
		}

		#endregion

		#region Methods      

		/// <summary>
		/// Handles the error.
		/// </summary>
		/// <param name="msg">Message.</param>
		public void HandleError(string msg)
		{
			throw new HttpRequestException(msg);
		}

		public void HandleInternetError()
		{
			throw new InternetException();
		}

		/// <summary>
		/// Handles the error async.
		/// </summary>
		public async Task<bool> HandleErrorAsync(HttpResponseMessage response, string obj, int attempt)
		{
			await Task.FromResult(true);

            if (response != null)
			{
                throw new HttpRequestException(((int)response.StatusCode).ToString());
            }
			else
			{
				throw new HttpRequestException("500");
			}
		}

		#endregion
	}
}
