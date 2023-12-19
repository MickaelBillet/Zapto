using Framework.Infrastructure.Services;
using System.Net;

namespace Zapto.Component.Services
{
    public class ErrorHandlerWebServiceZapto : IErrorHandlerWebService
	{
		#region Property

		#endregion

		#region Constructor

		public ErrorHandlerWebServiceZapto(IServiceProvider provider)
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
		/// <returns>The error async.</returns>
		/// <param name="stream">Stream.</param>
		/// <param name="obj">Object.</param>
		public async Task<bool> HandleErrorAsync(HttpResponseMessage response, string obj, int attempt)
		{
			bool hasError = true;

			if (response != null)
			{
				if (response.StatusCode != HttpStatusCode.NotFound)
				{
					throw new HttpRequestException(((int)response.StatusCode).ToString());
				}
			}
			else
			{
				throw new HttpRequestException("500");
			}

			return await Task.FromResult<bool>(hasError);
		}

		#endregion
	}
}
