using Connect.Mobile.Services;
using Connect.Model;
using Framework.Mobile.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Reflection;
using Xamarin.Essentials;

namespace Connect.Mobile
{
    public sealed class Host
	{
		#region Properties
		private IHost HostApplication { get; set; }
		public static Host Current { get; }
		static Host() { Current = new Host(); }
        private static string Environment { get; } = "Production"; //"Production" "Developpment"
		#endregion

		#region Constructor
		private Host()
		{				
		}
		#endregion

		#region Methods
		public void Initialize(Action<HostBuilderContext, IServiceCollection> nativeConfigureServices)
		{
			using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"Connect.Mobile.UserSettings.{Environment}.json"))
			{
				this.HostApplication = new HostBuilder().ConfigureHostConfiguration(configurationBuilder =>
				{
					// Tell the host configuration where is the file (this is required for Xamarin apps)
					configurationBuilder.AddCommandLine(new string[] {$"ContentRoot={FileSystem.AppDataDirectory}"});
					//read in the configuration file!
					configurationBuilder.AddJsonStream(stream);
				}).ConfigureServices((context, services) =>
				{
					services.AddHttpClient("ConnectClient", c =>
					{
						c.BaseAddress = new Uri($"{context.Configuration["ProtocolController"]}://{context.Configuration["BackEndUrl"]}:{context.Configuration["PortHttps"]}/{context.Configuration["PathBase"]}");
						c.Timeout = new TimeSpan(100000000); //10 sec
						c.DefaultRequestHeaders.Accept.Clear();
						c.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
						c.DefaultRequestHeaders.Add("X-Forwarded-Path", ConnectConstants.Application_Prefix);
					})
					.ConfigurePrimaryHttpMessageHandler(_ => new CustumHttpMessageHandler(context.Configuration["BackEndUrl"], "connect-zapto.fr"));

					services.AddHttpClient("Keycloak", c =>
					{
						c.Timeout = new TimeSpan(100000000); //10 sec
						c.DefaultRequestHeaders.Accept.Clear();
						c.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
					})
					.ConfigurePrimaryHttpMessageHandler(_ => new CustumHttpMessageHandler(context.Configuration["Keycloak:Authority"], "connect-zapto.fr"));

                    nativeConfigureServices(context, services);
					this.ConfigureServices(services, context.Configuration);
				})
				.Build();
			}
		}

		public T GetService<T>()
		{
			return HostApplication.Services.GetService<T>();
		}

        public T GetRequiredService<T>()
        {
            return HostApplication.Services.GetRequiredService<T>();
        }

        private void ConfigureServices(IServiceCollection services, IConfiguration configuration)
		{
			services.AddConnectServices(configuration);
			services.AddLogServices();
			services.AddViewModels();
		}
		#endregion
	}
}
