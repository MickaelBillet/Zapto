using Connect.Application;
using Connect.Application.Infrastructure;
using Connect.Infrastructure.Services;
using Connect.Mobile.Interfaces;
using Connect.Mobile.ViewModel;
using Framework.Infrastructure.Services;
using Framework.Mobile.Core.Services;
using Framework.Mobile.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using ErrorHandlerWebService = Framework.Mobile.Core.Services.ErrorHandlerWebService;
using IErrorHandlerService = Connect.Mobile.Interfaces.IErrorHandlerService;

namespace Connect.Mobile.Services
{
    public static class ServiceConfiguration
    {
        public static void AddConnectServices(this IServiceCollection services, IConfiguration configuration)
        {
            //Singleton is necessary for Authentication Web Service to keep the Token for each call
            services.AddSingleton<IAuthenticationWebService, AuthenticationWebService>((service) => new AuthenticationWebService(service, "Keycloak"));
			services.AddTransient<IHttpClientService, HttpClientService>((service) => new HttpClientService(service, configuration));			
			services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<IDialogService, DialogService>();
            services.AddSingleton<IErrorHandlerService, BaseViewModel>();
            services.AddTransient<ISignalRService, SignalRService>();
            services.AddTransient<IInternetService, InternetServiceMobile>();
			services.AddTransient<ICacheService, CacheService>();
			services.AddMailService();
            services.AddTransient<IErrorHandlerWebService, ErrorHandlerWebService>();
            services.AddThreadSynchronizationService();
            services.AddConnectWebServices(configuration, "ConnectClient");
			services.AddApplicationConnectServices();
		}

        public static void AddLogServices(this IServiceCollection services)
        {
			ILogger logger = new LoggerConfiguration()// Set default log level limit to Debug				
									.CreateLogger();

			services.AddSingleton<ILogger>(logger);

			Log.Logger = logger;
		}

		public static void AddViewModels(this IServiceCollection services)
		{
			services.AddSingleton<MainViewModel>();
			services.AddSingleton<MasterViewModel>();
			services.AddSingleton<DetailHomeViewModel>();
			services.AddTransient<DetailViewModel>();
			services.AddTransient<SettingsViewModel>();
			services.AddTransient<OperationRangeViewModel>();
			services.AddTransient<PlugCellViewModel>();
			services.AddTransient<SensorCellViewModel>();
			services.AddTransient<NotificationViewModel>();
		}
	}
}
