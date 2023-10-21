using Framework.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Zapto.Component.Common.Services;

namespace Zapto.Component.Services
{
    public class ImageService : IImageService
	{
		#region Property
		protected ILogger? Logger { get; set; }
		protected IWebService WebService { get; private set; }
		#endregion

		#region Constructor
		public ImageService(IServiceProvider serviceProvider, IConfiguration configuration, string httpClientName)
		{
			this.Logger = serviceProvider.GetService<ILogger>();
            this.WebService = WebServiceFactory.CreateWebService(serviceProvider, httpClientName);
        }
        #endregion

        #region Methods

        public async Task<Stream?> GetImageStreamAsync(string url)
		{
			Stream? res = null;

			try
			{
				res = await this.WebService.GetImageStreamAsync(url);
			}
			catch (Exception ex)
			{
				this.Logger?.Error(ex.Message);
			}

			return res;
		}

		#endregion
	}
}