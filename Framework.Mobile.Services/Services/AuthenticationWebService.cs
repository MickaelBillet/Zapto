using Framework.Core.Base;
using Framework.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Framework.Mobile.Core.Services
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
                HttpClient = httpClientService.GetClient(name);
            }
        }

        #endregion

        #region Methods
        public async Task<bool> GetTokenAsync(string url, string username, string password, string client_id, string client_secret)
        {
            bool result = false;
            if (HttpClient != null)
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
                    using (HttpResponseMessage response = await HttpClient.SendAsync(request).ConfigureAwait(false))
                    {
                        if (response != null)
                        {
                            if (response.IsSuccessStatusCode == true)
                            {
                                Stream stream = await response.Content.ReadAsStreamAsync();
                                Token = await JsonSerializer.DeserializeAsync<Token>(stream);
                                if (Token != null)
                                {
                                    ClientId = client_id;
                                    ClientSecret = client_secret;
                                    TokenEndpoint = url;
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
            if (HttpClient != null)
            {
                var content = new Dictionary<string, string>();
                content.Add("grant_type", "refresh_token");
                content.Add("refresh_token", Token != null ? Token.refresh_token : string.Empty);
                content.Add("client_id", ClientId != null ? ClientId : string.Empty);
                content.Add("client_secret", ClientSecret != null ? ClientSecret : string.Empty);

                using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, TokenEndpoint))
                {
                    request.Content = new FormUrlEncodedContent(content);
                    using (HttpResponseMessage response = await HttpClient.SendAsync(request).ConfigureAwait(false))
                    {
                        if (response != null)
                        {
                            if (response.IsSuccessStatusCode == true)
                            {
                                Stream stream = await response.Content.ReadAsStreamAsync();
                                Token = await JsonSerializer.DeserializeAsync<Token>(stream);
                                if (Token != null)
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
