using AirZapto.Application;
using AirZapto.Mobile.Resources;
using AirZapto.Model;
using Framework.Core;
using Framework.Core.Base;
using Framework.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Command = Xamarin.Forms.Command;

namespace AirZapto.ViewModel
{
    public class MainViewModel : BaseViewModel
	{
        private ObservableCollection<SensorViewModel> _sensorModels = null;
        private SensorViewModel currentSensorViewModel = null;

        #region Properties

        public Action ClosePopuUp;
       
        public ObservableCollection<SensorViewModel> SensorViewModels
        {
            get { return _sensorModels; }

            set 
            { 
                SetProperty<ObservableCollection<SensorViewModel>>(ref _sensorModels, value); 
            }
        }  

        public SensorViewModel CurrentSensorViewModel
		{
            get { return currentSensorViewModel; }

            set 
            { 
                SetProperty<SensorViewModel>(ref currentSensorViewModel, value);
                Debug.WriteLine(currentSensorViewModel?.Sensor.Description);

                this.CalibrationCommand.RaiseCanExecuteChanged();
                this.RestartCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region Commands 

        public ICommand CloseMenuCommand { get; }
        public AsyncCommand<object> RestartCommand { get; }
        public AsyncCommand<object> CalibrationCommand { get; }

        #endregion

        #region Services

        private IApplicationSensorServices ApplicationSensorServices { get; }
        private IServiceScopeFactory ScopeFactory { get; }
        private IAuthenticationWebService AuthenticationWebService { get; }
        private IConfiguration Configuration { get; }
        #endregion

        #region Constructor

        public MainViewModel(IServiceProvider serviceProvider, IConfiguration configuration) : base(serviceProvider)
		{
            this.CloseMenuCommand = new Command(() => ExecuteCloseMenuCommand());
            this.RestartCommand = new AsyncCommand<object>((obj) => ExecuteRestartCommand(obj), (obj) => CanRestartCommand(obj));
            this.CalibrationCommand = new AsyncCommand<object>((obj) => ExecuteCalibrationCommand(obj), (obj) => CanCalibrationCommand(obj));
            this.ApplicationSensorServices = serviceProvider.GetService<IApplicationSensorServices>();
            this.ScopeFactory = serviceProvider.GetService<IServiceScopeFactory>();
            this.AuthenticationWebService= serviceProvider.GetService<IAuthenticationWebService>();
            this.Configuration = configuration;
        }

        #endregion

        #region Method

        /// <summary>
        /// Initializes the async.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="item">Item.</param>
        public override async Task Initialize(Object item, Func<Object, Task> validateCallback = null, Func<Object, Task> cancelCallback = null)
        {
            try
            {
                this.LoadingTask = new NotifyTaskCompletion(Task.Run(async () =>
                {
                    try
                    {
                        bool res = await this.AuthenticationWebService.GetTokenAsync($"{this.Configuration["Keycloak:Authority"]}{this.Configuration["Keycloak:token_endpoint_url"]}",
                                                                                           this.Configuration["User"],
                                                                                           this.Configuration["Password"],
                                                                                           this.Configuration["Keycloak:client_id"],
                                                                                           this.Configuration["Keycloak:client_secret"]);

                        if (res == true)
                        {
                            IEnumerable<Sensor> sensors = await this.ApplicationSensorServices.GetSensorsAsync().ConfigureAwait(true);

                            this.SensorViewModels = new ObservableCollection<SensorViewModel>();

                            SensorViewModel model = null;

                            foreach (Sensor sensor in sensors)
                            {
                                using (IServiceScope scope = this.ScopeFactory.CreateScope())
                                {
                                    model = scope.ServiceProvider.GetService<SensorViewModel>();

                                    await model.Initialize(sensor);

                                    this.SensorViewModels.Add(model);
                                }
                            }

                            this.ErrorHandlerService.CleanError(this.HideError);
                        }
                        else
                        {
                            this.ErrorHandlerService.TriggerError(new Error(AppResources.ErrorWebService, Errors.ERR0000_Error, true, true), this.DisplayError);
                        }
                    }
                    catch (InternetException)
                    {
                        this.IsConnected = false;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                        this.ErrorHandlerService.TriggerError(new Error(AppResources.ErrorWebService, Errors.ERR0001_ErrorWebService, true, true), this.DisplayError);
                    }
                    finally
                    {
                        this.IsInitialized = true;
                    }
                }));

                await base.Initialize(null, validateCallback, cancelCallback);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                this.ErrorHandlerService.TriggerError(new Error(ex.Message, null, true, false), this.DisplayError);
            }
        }

        protected override void RefreshData()
        {
            this.LoadingTask = new NotifyTaskCompletion(Task.Run(async () =>
            {
                try
                {
                    Sensor sensor = await this.ApplicationSensorServices.GetSensorsAsync(this.CurrentSensorViewModel?.Sensor.Id).ConfigureAwait(true);
                    await this.CurrentSensorViewModel.Initialize(sensor);
                    this.ErrorHandlerService.CleanError(this.HideError);
                }
                catch(InternetException)
				{
                    this.IsConnected = false;
				}
                catch (Exception)
                {
                    this.ErrorHandlerService.TriggerError(new Error(AppResources.ErrorWebService, Errors.ERR0001_ErrorWebService, true, true), this.DisplayError);
                }
            }));
        }

        public void ExecuteCloseMenuCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                this.ClosePopuUp();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                this.ErrorHandlerService.TriggerError(new Error(ex.Message, null, true, false), this.DisplayErrorDialogBox);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public bool CanRestartCommand(object parameter)
        {
            return true;
        }

        public async Task ExecuteRestartCommand(object parameter)
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                this.ClosePopuUp();

                await this.NavigationService.PopupNavigateToAsync<RestartPopupViewModel>(this.CurrentSensorViewModel?.Sensor);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                this.ErrorHandlerService.TriggerError(new Error(ex.Message, null, true, false), this.DisplayErrorDialogBox);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public bool CanCalibrationCommand(object parameter)
        {
            return true;
        }

        public async Task ExecuteCalibrationCommand(object parameter)
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                this.ClosePopuUp();

                await this.NavigationService.PopupNavigateToAsync<CalibrationPopupViewModel>(this.CurrentSensorViewModel?.Sensor);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                this.ErrorHandlerService.TriggerError(new Error(ex.Message, null, true, false), this.DisplayErrorDialogBox);
            }
            finally
            {
                IsBusy = false;
            }
        }

        #endregion
    }
}
