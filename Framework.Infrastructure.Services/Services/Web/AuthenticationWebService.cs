using Framework.Core.Base;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Framework.Infrastructure.Services
{
    public class AuthenticationWebService : IAuthenticationWebService
	{
		#region Property

		private string TokenEndpoint { get; set; }

		private string ClientId { get; set; }

		private string ClientSecret { get; set; }

		public Token Token { get; set; }

		#endregion

		#region Services

		private HttpClient HttpClient { get; }

        #endregion

        #region Constructor

        public AuthenticationWebService(IServiceProvider serviceProvider, string name)
		{
            IHttpClientService httpClientService = serviceProvider.GetService<IHttpClientService>();
			if (httpClientService != null)
			{
				this.HttpClient = httpClientService.GetClient(name);
			}
		}

		#endregion

		#region Methods
		public async Task<bool> GetTokenAsync(string url, string username, string password, string client_id, string client_secret)
		{
			bool result = false;
			if (this.HttpClient != null)
			{
                var content = new Dictionary<string, string>();
                content.Add("grant_type", "password");
                content.Add("username", username);
                content.Add("password", password);
                content.Add("client_id", client_id);
                content.Add("client_secret", client_secret);

				using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url))
				{
					request.Content = new FormUrlEncodedContent(content);
                    using (HttpResponseMessage response = await this.HttpClient.SendAsync(request).ConfigureAwait(false))
					{
						if (response != null)
						{
							if (response.IsSuccessStatusCode == true)
							{
                                Stream stream = await response.Content.ReadAsStreamAsync();
                                this.Token = await JsonSerializer.DeserializeAsync<Token>(stream);
								if (this.Token != null)
								{
									this.ClientId = client_id;
									this.ClientSecret = client_secret;
									this.TokenEndpoint = url;
									result = true;
								}
                            }
						}
					}
				}
			}

            return result;
		}
        public async Task<bool> RefreshAccessToken()
        {
            bool result = false;
            if (this.HttpClient != null)
            {
                var content = new Dictionary<string, string>();
                content.Add("grant_type", "refresh_token");
                content.Add("refresh_token", (this.Token != null) ? this.Token.refresh_token : string.Empty);
                content.Add("client_id", (this.ClientId != null) ? this.ClientId : string.Empty);
				content.Add("client_secret", (this.ClientSecret != null) ? this.ClientSecret : string.Empty);

                using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, this.TokenEndpoint))
                {
                    request.Content = new FormUrlEncodedContent(content);
                    using (HttpResponseMessage response = await this.HttpClient.SendAsync(request).ConfigureAwait(false))
                    {
                        if (response != null)
                        {
                            if (response.IsSuccessStatusCode == true)
                            {
                                Stream stream = await response.Content.ReadAsStreamAsync();
                                this.Token = await JsonSerializer.DeserializeAsync<Token>(stream);
								if (this.Token != null)
								{
									result = true;
								}
                            }
                        }
                    }
                }
            }

			return result;
        }
        #endregion
    }
}
