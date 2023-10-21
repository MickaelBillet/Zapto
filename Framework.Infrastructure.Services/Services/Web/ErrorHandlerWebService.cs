using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Framework.Infrastructure.Services
{
    public class ErrorHandlerWebService : IErrorHandlerWebService
	{
		#region Property

		private IAuthenticationWebService AuthenticationService { get; }

		#endregion

		#region Constructor

		public ErrorHandlerWebService(IAuthenticationWebService service)
		{
			this.AuthenticationService = service;
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

			if ((response?.StatusCode == HttpStatusCode.Unauthorized || response?.StatusCode == HttpStatusCode.Forbidden) 
					&& (this.AuthenticationService != null) && (attempt == 1))
			{
				//We ask a new token
				bool isAuthenticated = await this.AuthenticationService.RefreshAccessToken();

				if (isAuthenticated != true)
				{
					throw new HttpRequestException(response.StatusCode + " - " + response.ReasonPhrase + " (" + obj + ") ");
				}
				else
				{
					hasError = false;
				}
			}
			else if (response?.StatusCode == HttpStatusCode.NotFound)
			{
				throw new HttpRequestException(response.StatusCode.ToString());
			}
			else
			{
				if (response != null)
				{
					throw new HttpRequestException(response.StatusCode 
													+ " - " + response.ReasonPhrase 
													+ " (" + obj + ") ");
				}
				else
				{
					throw new HttpRequestException("Unexpected error");
				}
			}

			return hasError;
		}

		#endregion
	}
}
