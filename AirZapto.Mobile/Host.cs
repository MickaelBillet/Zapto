using AirZapto.Application;
using AirZapto.Mobile.Interfaces;
using AirZapto.Mobile.Services;
using AirZapto.Services;
using AirZapto.ViewModel;
using AirZapto.Infrastructure.Services;
using Framework.Infrastructure.Services;
using Framework.Mobile.Core.Services;
using Framework.Mobile.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.Email;
using System;
using System.IO;
using System.Net;
using System.Reflection;
using Xamarin.Essentials;
using ErrorHandlerWebService = Framework.Mobile.Core.Services.ErrorHandlerWebService;

namespace AirZapto.Mobile
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
			using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"AirZapto.Mobile.UserSettings.{Environment}.json"))
			{
				this.HostApplication = new HostBuilder().ConfigureHostConfiguration(configurationBuilder =>
				{
					//Tell the host configuration where is the file (this is required for Xamarin apps)
					configurationBuilder.AddCommandLine(new string[] { $"ContentRoot={FileSystem.AppDataDirectory}" });

					//Read in the configuration file!
					configurationBuilder.AddJsonStream(stream);
				}).ConfigureServices((context, services) =>
				{
					services.AddHttpClient("AirZaptoClient", c =>
					{
						c.BaseAddress = new Uri($"{context.Configuration["ProtocolController"]}://{context.Configuration["BackEndUrl"]}:{context.Configuration["PortHttps"]}/{context.Configuration["PathBase"]}");
						c.Timeout = new TimeSpan(100000000); //10 sec
						c.DefaultRequestHeaders.Accept.Clear();
						c.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
						c.DefaultRequestHeaders.Add("X-Forwarded-Path", AirZaptoConstants.Application_Prefix);
					}).ConfigurePrimaryHttpMessageHandler(_ => new CustumHttpMessageHandler(context.Configuration["BackEndUrl"], "connect-zapto.fr"));

                    services.AddHttpClient("Keycloak", c =>
                    {
                        c.Timeout = new TimeSpan(100000000); //10 sec
                        c.DefaultRequestHeaders.Accept.Clear();
                        c.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                    })
                    .ConfigurePrimaryHttpMessageHandler(_ => new CustumHttpMessageHandler(context.Configuration["Keycloak:Authority"], "connect-zapto.fr"));

                    nativeConfigureServices(context, services);

					this.ConfigureServices(services, context.Configuration);
					this.ConfigureViewModels(services);
				})
				.Build();
			}
		}

		public T GetService<T>()
		{
			return HostApplication.Services.GetService<T>();
		}

		private void ConfigureServices(IServiceCollection services, IConfiguration configuration)
		{
            //Singleton is necessary for Authentication Web Service to keep the Token for each call
            services.AddSingleton<IAuthenticationWebService, AuthenticationWebService>((service) => new AuthenticationWebService(service, "Keycloak"));
            services.AddTransient<IHttpClientService, HttpClientService>((service) => new HttpClientService(service, configuration));
			services.AddSingleton<INavigationService, NavigationService>();
			services.AddSingleton<IDialogService, DialogService>();
			services.AddSingleton<IErrorHandlerService, ErrorHandlerService>();
			services.AddTransient<IInternetService, InternetServiceMobile>();
            services.AddTransient<IErrorHandlerWebService, ErrorHandlerWebService>();
            services.AddAirZaptoWebServices(configuration, "AirZaptoClient");
			services.AddApplicationAirZaptoServices();
            this.ConfigureLogs(services);
        }

        private void ConfigureViewModels(IServiceCollection services)
		{
			services.AddSingleton<MainViewModel>();
			services.AddTransient<CalibrationPopupViewModel>();
			services.AddTransient<RestartPopupViewModel>();
			services.AddTransient<SensorViewModel>();
		}

		public void ConfigureLogs(IServiceCollection services)
		{
			ILogger logger = new LoggerConfiguration()// Set default log level limit to Debug				
			.WriteTo.Logger(config => config
							.MinimumLevel.Error()
							.WriteTo.Email(new EmailConnectionInfo
							{
								MailServer = "smtp.gmail.com",
								FromEmail = "mickaconnect.zapto@gmail.com",
								ToEmail = "mickaconnect.zapto@gmail.com",
								EmailSubject = "Fatal error",
								NetworkCredentials = new NetworkCredential
								{
									UserName = "mickaconnect.zapto@gmail.com",
									Password = "beauTemps?08"
								},
								EnableSsl = true,
								Port = 465,
							}))
			.CreateLogger();

			services.AddSingleton<ILogger>(logger);

			Log.Logger = logger;
		}

		#endregion
	}
}
