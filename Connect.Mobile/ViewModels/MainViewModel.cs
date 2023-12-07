using Connect.Application;
using Connect.Mobile.Interfaces;
using Connect.Mobile.Resources;
using Connect.Model;
using Framework.Core.Base;
using Framework.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Connect.Mobile.ViewModel
{
    public class MainViewModel : BaseViewModel
	{
        private Location location = null;

        #region Properties

        public Location Location
        {
            get { return location; }
            set { SetProperty(ref location, value); }
        }

        public short ServiceType { get; } = ServiceAlertType.Firebase;

        #endregion

        #region Services
        private IApplicationConnectLocationServices ApplicationLocationServices { get; }
        private IAuthenticationWebService AuthenticationWebService { get; }
        private IConfiguration Configuration { get; }  
        private IForegroundService ForegroundService { get; }
        private IFirebaseMobileService FirebaseMobileService { get; }
        private IThreadSynchronizationService ThreadSynchronizationService { get; }
        #endregion

        #region Constructor

        public MainViewModel(IServiceProvider serviceProvider, IConfiguration configuration) : base(serviceProvider)
		{
            this.ApplicationLocationServices = serviceProvider.GetRequiredService<IApplicationConnectLocationServices>();
            this.AuthenticationWebService = serviceProvider.GetRequiredService<IAuthenticationWebService>();
            this.ForegroundService = serviceProvider.GetRequiredService<IForegroundService>();
            this.FirebaseMobileService = serviceProvider.GetRequiredService<IFirebaseMobileService>();
            this.ThreadSynchronizationService = serviceProvider.GetRequiredService<IThreadSynchronizationService>();
            this.Configuration = configuration;
        }

        #endregion

        #region Method

        /// <summary>
        /// Initializes the async.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="item">Item.</param>
        public override void Initialize(Object item, Func<Object, Task> validateCallback = null, Func<Object, Task> cancelCallback = null)
        {
            this.LoadingTask = new NotifyTaskCompletion(Task.Run(async () =>
            {
                IEnumerable<Location> locations = null;
                IEnumerable<Location> cache = null;

                try
                {
                    bool isAuthenticated = await this.AuthenticationWebService.GetTokenAsync($"{this.Configuration["Keycloak:Authority"]}{this.Configuration["Keycloak:token_endpoint_url"]}",
                                                                                                   this.Configuration["User"],
                                                                                                   this.Configuration["Password"],
                                                                                                   this.Configuration["Keycloak:client_id"],
                                                                                                   this.Configuration["Keycloak:client_secret"]);

                    if (isAuthenticated == true)
                    {
                        try
                        {
                            cache = await this.ApplicationLocationServices.GetLocationCache();
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex);
                            this.HandleError(Model.ErrorType.ErrorSoftware, ex.Message);
                        }
                        finally
                        {
                            try
                            {
                                locations = await this.ApplicationLocationServices.GetLocations();

                                if (locations == null)
                                {
                                    locations = cache;
                                }
                            }
                            finally
                            {
                                if ((locations != null) && (locations.Any()))
                                {
                                    this.Location = locations.First();
                                    App.LocationId = this.Location.Id;

                                    MasterViewModel masterViewModel = Host.Current.GetService<MasterViewModel>();
                                    masterViewModel.Initialize(this.Location);

                                    DetailHomeViewModel detailHomeViewModel = Host.Current.GetService<DetailHomeViewModel>();
                                    detailHomeViewModel.Initialize(this.location);

                                    if (this.ServiceType == ServiceAlertType.SignalR)
                                    {
                                        this.ForegroundService.StartForegroundServiceCompat();
                                    }
                                    else if (this.ServiceType == ServiceAlertType.Firebase)
                                    {
                                        bool res = this.ThreadSynchronizationService.Set("IFirebaseMobileService");
                                        this.FirebaseMobileService.ReceiveNotification();
                                    }
                                }
                                else
                                {
                                    this.HandleError(Model.ErrorType.Warning, AppResources.NoElement);
                                }

                                base.Initialize(null, validateCallback, cancelCallback);
                            }
                        }
                    }
                    else
                    {
                        this.HandleError(Model.ErrorType.ErrorWebService, AppResources.ErrorAuthentication);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    this.HandleError(Model.ErrorType.ErrorWebService, ex.Message);
                }
            }));
		}
        #endregion
    }
}