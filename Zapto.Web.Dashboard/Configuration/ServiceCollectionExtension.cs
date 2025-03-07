﻿using AirZapto.Application;
using AirZapto.Infrastructure.Services;
using Connect.Application;
using Connect.Application.Infrastructure;
using Connect.Infrastructure.Services;
using Framework.Infrastructure.Services;
using WeatherZapto.Application;
using WeatherZapto.Infrastructure.Services;
using Zapto.Component.Common.Services;
using Zapto.Component.Common.ViewModels;
using Zapto.Component.Services;

namespace Zapto.Web.Dashboard.Configuration
{
    public static class ServiceCollectionExtension
	{
		public static void AddViewModels(this IServiceCollection services)
		{
			services.AddTransient<IDashboardViewModel, DashboardViewModel>();
			services.AddTransient<IHealthCheckViewModel, HealthCheckViewModel>();
			services.AddTransient<IRoomListViewModel, RoomListViewModel>();
			services.AddTransient<IRoomViewModel, RoomViewModel>();
			services.AddTransient<ISensorDataListViewModel, SensorDataListViewModel>();
			services.AddTransient<IRoomChartViewModel, RoomChartViewModel>();
			services.AddTransient<ISelectDateViewModel,  SelectDateViewModel>();
            services.AddTransient<ISensorEventListViewModel, SensorEventListViewModel>();
            services.AddTransient<ISensorDataViewModel, SensorDataViewModel>();
            services.AddTransient<ISensorEventViewModel, SensorEventViewModel>();
            services.AddTransient<IPlugViewModel, PlugViewModel>();
            services.AddTransient<IPlugListViewModel, PlugListViewModel>();
            services.AddTransient<IAccessControlViewModel, AccessControlViewModel>();
			services.AddTransient<IAuthenticationViewModel, AuthenticationViewModel>();
			services.AddTransient<IMainViewModel, MainViewModel>();
			services.AddTransient<ISensorCO2ViewModel, SensorCO2ViewModel>();
			services.AddTransient<IAirPollutionViewModel, AirPollutionViewModel>();
			services.AddTransient<IChooseLanguageViewModel, ChooseLanguageViewModel>();
			services.AddTransient<IWeatherViewModel, WeatherViewModel>();
			services.AddTransient<ILocationViewModel, LocationViewModel>();
			services.AddTransient<IHomeViewModel, HomeViewModel>();
			services.AddTransient<ILocationSearchViewModel, LocationSearchViewModel>();
		}

		public static void AddServices(this IServiceCollection services, IConfiguration configuration)
		{
            services.AddTransient<IHttpClientService, HttpClientService>((service) => new HttpClientService(service, configuration));
			services.AddTransient<IInternetService, InternetServiceWeb>();
            services.AddConnectWebServices(configuration, "ConnectClient");
			services.AddAirZaptoWebServices(configuration, "AirZaptoClient");
			services.AddOpenWeatherWebServices(configuration, "OpenWeather");
			services.AddWeatherZaptoWebServices(configuration, "WeatherZaptoClient");
            services.AddApplicationAirZaptoServices();
            services.AddApplicationConnectServices();
			services.AddApplicationWeaterZaptoServices();
            services.AddTransient<ISignalRService, SignalRService>();
			services.AddScoped<INavigationService, NavigationService>();
			services.AddTransient<IPositionService, PositionService>();
			services.AddTransient<IAuthenticationService, AuthenticationService>();
			services.AddTransient<IErrorHandlerWebService, ErrorHandlerWebServiceZapto>();			
			services.AddTransient<IImageService, ImageService>((service) => new ImageService(service, configuration, "OpenWeather"));
			services.AddScoped<DataService>();
			services.AddScoped<IZaptoStorageService, AdpaterLocalStorageService>();
			services.AddScoped<ILocalStorageService, LocalStorageService>();
        }
    }
}
