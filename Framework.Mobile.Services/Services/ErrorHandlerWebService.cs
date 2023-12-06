using Framework.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Framework.Mobile.Core.Services
{
    public class ErrorHandlerWebService : IErrorHandlerWebService
    {
        #region Property

        private IAuthenticationWebService? AuthenticationService { get; }

        #endregion

        #region Constructor

        public ErrorHandlerWebService(IServiceProvider provider)
        {
            AuthenticationService = provider.GetService<IAuthenticationWebService>();
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
                    && AuthenticationService != null && attempt == 1)
            {
                //We ask a new token
                bool isAuthenticated = await AuthenticationService.RefreshAccessToken();

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
