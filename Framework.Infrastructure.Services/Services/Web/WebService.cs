using Framework.Core.Base;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Infrastructure.Services
{
    public class WebService : IWebService
    {
        #region Services
        private IErrorHandlerWebService ErrorHandlerService { get; }
        private IHttpClientService HttpClientService { get; }
        private IInternetService InternetService { get; }
        private IAuthenticationWebService AuthenticationWebService { get; }
        public string HttpClientName { get;}
        #endregion

        #region Property
        private SystemTextJsonContentSerializer SystemTextJsonContentSerializer { get; }
        #endregion

        #region Constructor
        public WebService(IServiceProvider serviceProvider, string httpClientName)
        {
            this.HttpClientService = serviceProvider.GetRequiredService<IHttpClientService>();
            this.InternetService = serviceProvider.GetRequiredService<IInternetService>();
            this.AuthenticationWebService = serviceProvider.GetService<IAuthenticationWebService>();
            this.HttpClientName = httpClientName;
            this.ErrorHandlerService = serviceProvider.GetService<IErrorHandlerWebService>();
            this.SystemTextJsonContentSerializer = new SystemTextJsonContentSerializer();
		}
		#endregion

		#region Method

        /// <summary>
        /// Gets the async.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="url">URL.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<IEnumerable<T>> GetCollectionAsync<T>(string url, JsonSerializerOptions options, CancellationToken cancellationToken = default, int attempt = 1)
        {
            IEnumerable<T> items = null;

           if (this.InternetService?.IsConnectedToInternet() != false)
            {
                HttpClient client = this.HttpClientService.GetClient(this.HttpClientName);

                if ((this.AuthenticationWebService != null) && (this.AuthenticationWebService.Token != null))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.AuthenticationWebService?.Token.access_token);
                }

                using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"{client.BaseAddress}{url}"))
                {
                    //Do not wait until all data is in memory before deserializing -> HttpCompletionOption.ResponseHeadersRead
                    using (HttpResponseMessage response = await this.QueryCurrencyServiceWithRetryPolicy(() => client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken), cancellationToken).ConfigureAwait(false))
                    {
                        //GET
                        if (response != null)
                        {
                            if (response.IsSuccessStatusCode == true)
                            {
                                Stream stream = await response.Content.ReadAsStreamAsync();
                                items = await this.SystemTextJsonContentSerializer.DeserializeAsync<IEnumerable<T>>(stream, options);
                            }
                            else
                            {
                                bool hasError = await this.ErrorHandlerService.HandleErrorAsync(response, $"{client.BaseAddress}{url}", attempt);

                                if (!hasError)
                                {
                                    items = await this.GetCollectionAsync<T>(url, options, cancellationToken, attempt: 2);
                                }
                            }
                        }
                        else if ((response == null) && (cancellationToken.IsCancellationRequested == false))
                        {
                            this.ErrorHandlerService.HandleError("HttpResponseFailure");
                        }
                    }
                }
            }
			else
			{
				this.ErrorHandlerService.HandleInternetError();
			}

			return items;
		}

        /// <summary>
        /// Head async
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serverPath"></param>
        /// <param name="url"></param>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="attempt"></param>
        /// <returns></returns>
        public async Task<bool?> HeadAsync<T>(string url, string id, CancellationToken cancellationToken = default, int attempt = 1)
        {
            bool? res = null;

            if (this.InternetService?.IsConnectedToInternet() != false)
            {
                HttpClient client = this.HttpClientService.GetClient(this.HttpClientName);

                if ((this.AuthenticationWebService != null) && (this.AuthenticationWebService.Token != null))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.AuthenticationWebService?.Token.access_token);
                }

                using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Head, $"{client.BaseAddress}{url}" + id))
                {
                    //HEAD
                    using (HttpResponseMessage response = await this.QueryCurrencyServiceWithRetryPolicy(() => client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken), cancellationToken).ConfigureAwait(false))
                    {
                        if (response != null)
                        {                            
                            if (response.IsSuccessStatusCode == true)
                            {
                                res = true;
                            }
                            else
                            {
                                bool hasError = await this.ErrorHandlerService.HandleErrorAsync(response, $"{client.BaseAddress}{url}" + id, attempt);
                                if (hasError == false)
                                {
                                    res = await this.HeadAsync<T>(url, id, cancellationToken, attempt: 2);
                                }
                                else
								{
                                    res = false;
								}
                            }
                        }
                        else if ((response == null) && (cancellationToken.IsCancellationRequested == false))
                        {
                            res = false;
                            this.ErrorHandlerService.HandleError("HttpResponseFailure");
                        }
                    }
                }
            }
			else
			{
				this.ErrorHandlerService.HandleInternetError();
			}

			return res;
		}

        /// <summary>
        /// Gets the async.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="url">URL.</param>
        /// <param name="id">Identifier.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
		public async Task<T> GetAsync<T>(string url, string id, JsonSerializerOptions options, CancellationToken cancellationToken = default, int attempt = 1) where T : new()
        {
            T item = default(T);

            if (this.InternetService?.IsConnectedToInternet() != false)
            {
                HttpClient client = this.HttpClientService.GetClient(this.HttpClientName);

                if ((this.AuthenticationWebService != null) && (this.AuthenticationWebService.Token != null))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.AuthenticationWebService?.Token.access_token);
                }

                using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, string.Format($"{client.BaseAddress}{url}", id)))
                {
                    //Do not wait until all data is in memory before deserializing -> HttpCompletionOption.ResponseHeadersRead
                    using (HttpResponseMessage response = await this.QueryCurrencyServiceWithRetryPolicy(() => client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken), cancellationToken).ConfigureAwait(false))
                    {
                        if (response != null)
                        {
                            if (response.IsSuccessStatusCode == true)
                            {
                                Stream stream = await response.Content.ReadAsStreamAsync();
                                item = await this.SystemTextJsonContentSerializer.DeserializeAsync<T>(stream, options);
                            }
                            else
                            {
                                bool hasError = await this.ErrorHandlerService.HandleErrorAsync(response, $"{client.BaseAddress}{url}" + id, attempt);

                                if (hasError == false)
                                {
                                    item = await this.GetAsync<T>(url, id, options, cancellationToken, attempt: 2);
                                }
                            }
                        }
                        else if ((response == null) && (cancellationToken.IsCancellationRequested == false))
                        {
                            this.ErrorHandlerService.HandleError("HttpResponseFailure");
                        }
                    }
                }                
            }
			else
			{
				this.ErrorHandlerService.HandleInternetError();
			}

			return item;
		}

        /// <summary>
        /// Posts the async.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="url">URL.</param>
        /// <param name="t">T.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<bool?> PostAsync<T>(string url, T t, CancellationToken cancellationToken = default, int attempt = 1)
        {
            bool? res = null;

            if (this.InternetService?.IsConnectedToInternet() != false)
            {
                HttpClient client = this.HttpClientService.GetClient(this.HttpClientName);

                if ((this.AuthenticationWebService != null) && (this.AuthenticationWebService.Token != null)) 
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.AuthenticationWebService?.Token.access_token);
                }

                MemoryStream memoryStream = await this.SystemTextJsonContentSerializer.SerializeWithStreamAsync<T>(t);

                //Prepares the message of the call webservice
                using (StreamContent requestContent = new StreamContent(memoryStream))
                {
                    using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"{client.BaseAddress}{url}"))
                    {
                        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        request.Content = requestContent;
                        requestContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                        //POST
                        using (HttpResponseMessage response = await this.QueryCurrencyServiceWithRetryPolicy(() => client.SendAsync(request, cancellationToken), cancellationToken).ConfigureAwait(false))
                        {
                            if (response != null)
                            {
                                using (Stream stream = await response.Content.ReadAsStreamAsync())
                                {
                                    if (response.IsSuccessStatusCode == true)
                                    {
                                        res = true;
                                    }
                                    else
                                    {
                                        bool hasError = await this.ErrorHandlerService.HandleErrorAsync(response, $"{client.BaseAddress}{url}", attempt);

                                        if (!hasError)
                                        {
                                            res = await this.PostAsync<T>(url, t, cancellationToken, attempt: 2);
                                        }
                                        else
                                        {
                                            res = false;
                                        }
                                    }
                                }
                            }
                            else if ((response == null) && (cancellationToken.IsCancellationRequested == false))
                            {
                                res = false;
                                this.ErrorHandlerService.HandleError("HttpResponseFailure");
                            }
                        }
                    }
                }
            }
			else
			{
				this.ErrorHandlerService.HandleInternetError();
			}

			return res;
		}

        /// <summary>
        /// Puts the async.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="url">URL.</param>
        /// <param name="t">T.</param>
        /// <param name="id">Identifier.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<bool?> PutAsync<T>(string url, T t, string id, CancellationToken cancellationToken = default, int attempt = 1)
        {
            bool? res = null;

            if (this.InternetService?.IsConnectedToInternet() != false)
            {
                HttpClient client = this.HttpClientService.GetClient(this.HttpClientName);

                if ((this.AuthenticationWebService != null) && (this.AuthenticationWebService.Token != null))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.AuthenticationWebService?.Token.access_token);
                }

                MemoryStream memoryStream = await this.SystemTextJsonContentSerializer.SerializeWithStreamAsync<T>(t);

                //Prepares the message of the call webservice
                using (StreamContent requestContent = new StreamContent(memoryStream))
                {
                    using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, string.Format($"{client.BaseAddress}{url}", id)))
                    {
                        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        request.Content = requestContent;
                        requestContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                        //PUT
                        using (HttpResponseMessage response = await this.QueryCurrencyServiceWithRetryPolicy(() => client.SendAsync(request, cancellationToken), cancellationToken).ConfigureAwait(false))
                        {
                            if (response != null)
                            {
                                using (Stream stream = await response.Content.ReadAsStreamAsync())
                                {
                                    if (response.IsSuccessStatusCode == true)
                                    {
                                        res = true;
                                    }
                                    else
                                    {
                                        bool hasError = await this.ErrorHandlerService.HandleErrorAsync(response, $"{client.BaseAddress}{url}" + id, attempt);

                                        if (!hasError)
                                        {
                                            res = await this.PutAsync<T>(url, t, id, cancellationToken, attempt: 2);
                                        }
                                        else
                                        {
                                            res = false;
                                        }
                                    }
                                }
                            }
                            else if ((response == null) && (cancellationToken.IsCancellationRequested == false))
                            {
                                res = false;
                                this.ErrorHandlerService.HandleError("HttpResponseFailure");
                            }
                        }
                    }
                }
            }
			else
			{
				this.ErrorHandlerService.HandleInternetError();
			}

			return res;
		}

        /// <summary>
        /// Deletes the async.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="url">URL.</param>
        /// <param name="id">Identifier.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
		public async Task<bool?> DeleteAsync<T>(string url, string id, CancellationToken cancellationToken = default, int attempt = 1)
        {
            bool? res = null;

            if (this.InternetService?.IsConnectedToInternet() != false)
            {
                HttpClient client = this.HttpClientService.GetClient(this.HttpClientName);

                if ((this.AuthenticationWebService != null) && (this.AuthenticationWebService.Token != null))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.AuthenticationWebService?.Token.access_token);
                }

                using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, string.Format($"{client.BaseAddress}{url}", id)))
                {
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //DELETE
                    using (HttpResponseMessage response = await this.QueryCurrencyServiceWithRetryPolicy(() => client.SendAsync(request, cancellationToken), cancellationToken).ConfigureAwait(false))
                    {
                        if (response != null)
                        {
                            using (Stream stream = await response.Content.ReadAsStreamAsync())
                            {
                                if (response.IsSuccessStatusCode == true)
                                {
                                    res = true;
                                }
                                else
                                {
                                    bool hasError = await this.ErrorHandlerService.HandleErrorAsync(response, $"{client.BaseAddress}{url}" + id, attempt);

                                    if (!hasError)
                                    {
                                        res = await this.DeleteAsync<T>(url, id, cancellationToken, attempt: 2);
                                    }
                                    else
                                    {
                                        res = false;
                                    }
                                }
                            }
                        }
                        else if ((response == null) && (cancellationToken.IsCancellationRequested == false))
                        {
                            res = false;
                            this.ErrorHandlerService.HandleError("HttpResponseFailure");
                        }
                    }
                }
            }
			else
			{
				this.ErrorHandlerService.HandleInternetError();
			}

			return res;
		}

        public async Task<Stream> GetImageStreamAsync(string url)
        {
			Stream res = null;

			if (this.InternetService?.IsConnectedToInternet() != false)
			{
				HttpClient client = this.HttpClientService.GetClient(this.HttpClientName);

                if ((this.AuthenticationWebService != null) && (this.AuthenticationWebService.Token != null))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.AuthenticationWebService?.Token.access_token);
				}

                res = await client.GetStreamAsync(url);									
			}
			else
			{
				this.ErrorHandlerService.HandleInternetError();
			}

			return res;
		}

		private async Task<HttpResponseMessage> QueryCurrencyServiceWithRetryPolicy(Func<Task<HttpResponseMessage>> action, CancellationToken token)
		{
			int numberOfTimesToRetry = 7;
			int retryMultiple = 2;
			HttpResponseMessage response = null;

			try
			{
				//Handle HttpRequestException when it occures
				response = await Policy.Handle<HttpRequestException>(ex =>
				{
					Debug.WriteLine("Request failed due to connectivity issues.");
					return true;
				})

				//wait for a given number of seconds which increases after each retry
				.WaitAndRetryAsync(numberOfTimesToRetry, retryCount => TimeSpan.FromSeconds(retryCount * retryMultiple))

				//After the retry, Execute the appropriate set of instructions
				.ExecuteAsync(async (ct) =>
                {
                    ct.ThrowIfCancellationRequested();
                    return await action();
                }, token);
			}
			catch (TaskCanceledException)
			{
				this.ErrorHandlerService.HandleError("HttpResponseFailure");
			}
            catch (OperationCanceledException)
			{
                //We cancel the task
            }

			//Return the response message gotten from the http client call.
			return response;
		}

		#endregion
	}
}
