using AirZapto.Application;
using AirZapto.Application.Infrastructure;
using Blazored.LocalStorage;
using Connect.Application;
using Connect.Application.Infrastructure;
using Framework.Infrastructure.Services;
using Microsoft.AspNetCore.Components.Authorization;
using WeatherZapto.Application;
using Zapto.Component.Common.Services;
using IApplicationSensorServices = Connect.Application.IApplicationSensorServices;

namespace Zapto.Web.Test.ViewModels
{
    public class BaseViewModelUnitTest
    {
        protected Mock<INavigationService> NavigationService { get; }
        protected Mock<AuthenticationStateProvider> AuthenticationStateProvider { get; }
        protected Mock<IAuthenticationService> AuthenticationService { get; }
        protected IServiceCollection ServiceCollection { get; }
        protected Mock<ILocationViewModel> LocationViewModel { get; }
        protected Mock<IWeatherViewModel> WeatherViewModel { get; }
        protected Mock<IApplicationLocationServices> ApplicationLocationServices { get; }
        protected Mock<IApplicationHealthCheckConnectServices> ApplicationHealthCheckConnectServices { get; }
        protected Mock<IApplicationHealthCheckAirZaptoServices> ApplicationHealthCheckAirZaptoServices { get; }
        protected Mock<ILocalStorageService> LocalStorageService { get; }
        protected Mock<IApplicationWeatherService> ApplicationWeatherService { get; }
        protected Mock<IApplicationPlugServices> ApplicationPlugServices { get; }
        protected Mock<IApplicationSensorServices> ApplicationSensorServices { get; }
        protected Mock<ILocationService> LocationService { get; }
        protected Mock<ICacheService> CacheService { get; }
        protected Mock<IHealthCheckAirZaptoService> HealthCheckAirZaptoService { get; }
        protected Mock<IHealthCheckConnectService> HealthCheckConnectService { get; }
        protected Mock<ISignalRService> SignalRService { get; }

        public BaseViewModelUnitTest() 
        {
            this.NavigationService = new Mock<INavigationService>();
            this.AuthenticationStateProvider = new Mock<AuthenticationStateProvider>(); 
            this.AuthenticationService = new Mock<IAuthenticationService>();
            this.LocationViewModel = new Mock<ILocationViewModel>();
            this.WeatherViewModel = new Mock<IWeatherViewModel>();
            this.LocalStorageService = new Mock<ILocalStorageService>();
            this.LocationService = new Mock<ILocationService>();
            this.CacheService = new Mock<ICacheService>();
            this.HealthCheckAirZaptoService = new Mock<IHealthCheckAirZaptoService>();
            this.HealthCheckConnectService = new Mock<IHealthCheckConnectService>();
            this.ApplicationHealthCheckConnectServices = new Mock<IApplicationHealthCheckConnectServices>();
            this.ApplicationHealthCheckAirZaptoServices = new Mock<IApplicationHealthCheckAirZaptoServices>();
            this.ApplicationLocationServices = new Mock<IApplicationLocationServices>();
            this.ApplicationWeatherService = new Mock<IApplicationWeatherService>();
            this.ApplicationSensorServices = new Mock<IApplicationSensorServices>();
            this.SignalRService = new Mock<ISignalRService>();
            this.ApplicationPlugServices = new Mock<IApplicationPlugServices>();

            this.ServiceCollection = new ServiceCollection();
            this.ServiceCollection.AddTransient<ILocationViewModel>(_ => this.LocationViewModel.Object);
            this.ServiceCollection.AddTransient<INavigationService>(_ => this.NavigationService.Object);
            this.ServiceCollection.AddTransient<AuthenticationStateProvider>(_ => this.AuthenticationStateProvider.Object);
            this.ServiceCollection.AddTransient<IAuthenticationService>(_ => this.AuthenticationService.Object);
            this.ServiceCollection.AddTransient<IWeatherViewModel>(_ => this.WeatherViewModel.Object);
            this.ServiceCollection.AddTransient<IApplicationLocationServices>(_ => this.ApplicationLocationServices.Object);
            this.ServiceCollection.AddTransient<ILocalStorageService>(_ => this.LocalStorageService.Object);
            this.ServiceCollection.AddTransient<IApplicationWeatherService>(_ => this.ApplicationWeatherService.Object);
            this.ServiceCollection.AddTransient<ILocationService>(_ => this.LocationService.Object);
            this.ServiceCollection.AddTransient<ICacheService>(_ => this.CacheService.Object);
            this.ServiceCollection.AddTransient<IApplicationHealthCheckConnectServices>(_ => this.ApplicationHealthCheckConnectServices.Object);    
            this.ServiceCollection.AddTransient<IApplicationHealthCheckAirZaptoServices>(_ => this.ApplicationHealthCheckAirZaptoServices.Object);
            this.ServiceCollection.AddTransient<IHealthCheckAirZaptoService>(_ => this.HealthCheckAirZaptoService.Object);
            this.ServiceCollection.AddTransient<IHealthCheckConnectService>(_ => this.HealthCheckConnectService.Object);
            this.ServiceCollection.AddTransient<ISignalRService>(_ => this.SignalRService.Object);
            this.ServiceCollection.AddTransient<IApplicationPlugServices>(_ => this.ApplicationPlugServices.Object);
            this.ServiceCollection.AddTransient<IApplicationSensorServices>(_ => this.ApplicationSensorServices.Object);
        }
    }
}
