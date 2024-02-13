using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

namespace Framework.Infrastructure.Services
{
    public class HttpClientService : IHttpClientService
	{
		#region Properties
		private IHttpClientFactory ClientFactory { get; }
		#endregion

		#region Constructor
		public HttpClientService(IServiceProvider serviceProvider, IConfiguration configuration)
		{
			this.ClientFactory = serviceProvider.GetService<IHttpClientFactory>();
		}
		#endregion

		#region Methods
		public HttpClient GetClient(string name)
		{
			return this.ClientFactory?.CreateClient(name);
		}
		#endregion
	}
}
