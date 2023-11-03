using AirZapto.Mobile.Interfaces;
using AirZapto.Mobile.Resources;
using AirZapto.Model;
using Framework.Core;
using Framework.Core.Base;
using Framework.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Command = Xamarin.Forms.Command;

namespace AirZapto.ViewModel
{
    public class BaseViewModel : ObservableObject, IDisposable, IPageLifeCylceEvents
    {
        // some fields that require cleanup
        private SafeHandle handle = null;
		private bool isBusy = false;        
		private string title = string.Empty;
        private bool isConnected = (Connectivity.NetworkAccess == NetworkAccess.Internet);
        private bool isRefreshing;
        private bool isInitialized = false;
        private NotifyTaskCompletion loadingtask = null;

        #region Command

        public ICommand SignOutCommand { get; }

        public AsyncCommand<object> RefreshCommand { get; set; }

        #endregion

        #region Property

        public static ClientApp ClientApp { get; set; }

		private bool Disposed { get; set; } = false; // to detect redundant calls

        public NotifyTaskCompletion LoadingTask
        {
            get { return loadingtask; }

            protected set { SetProperty(ref loadingtask, value); }
        }

        public bool IsRefreshing
        {
            get { return isRefreshing; }

            set 
            { 
                SetProperty(ref isRefreshing, value);
                this.RefreshCommand.RaiseCanExecuteChanged();
            }
        }

        public bool IsBusy
        {
            get { return isBusy; }

            set
            {
                SetProperty(ref isBusy, value);
                this.RefreshCommand.RaiseCanExecuteChanged();
            }
        }

        public bool IsInitialized
        {
            get { return isInitialized; }

            set
            {
                SetProperty(ref isInitialized, value);
            }
        }

        /// <summary>
        /// Public property to set and get the title of the item
        /// </summary>
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

		protected INavigationService NavigationService { get; }
        protected IDialogService DialogService { get; }
        protected IErrorHandlerService ErrorHandlerService { get; }
        protected ILogger Logger { get; }

        public bool IsConnected
		{
			get { return isConnected; }
			protected set { SetProperty(ref isConnected, value); }
		}

        protected Func<Object, Task> CancelCallback { get; private set; }

        protected Func<Object, Task> ValidateCallback { get; private set; }

        #endregion

        #region Constructor

        public BaseViewModel(IServiceProvider serviceProvider)
        {
            this.NavigationService = serviceProvider.GetService<INavigationService>();
            this.DialogService = serviceProvider.GetService<IDialogService>();
            this.Logger = serviceProvider.GetService<ILogger>();
            this.ErrorHandlerService = serviceProvider.GetService<IErrorHandlerService>();

            this.SignOutCommand = new Command(() => ExecuteSignOutCommand());
            this.RefreshCommand = new AsyncCommand<object>((obj) => ExecuteRefreshCommand(obj), (obj) => CanExecuteRefreshCommand(obj));

            Connectivity.ConnectivityChanged += CurrentConnectivityChanged;
        }

        #endregion

        #region Method

        /// <summary>
        /// Initializes the async.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="item">Item.</param>
        /// <param name="validateCallback">Validate callback.</param>
        /// <param name="cancelCallback">Cancel callback.</param>
        public virtual async Task Initialize(Object item, Func<Object, Task> validateCallback = null, Func<Object, Task> cancelCallback = null)
		{
            if (validateCallback != null)
            {
                this.ValidateCallback = validateCallback;
            }

            if (cancelCallback != null)
            {
                this.CancelCallback = cancelCallback;
            }

            await Task.CompletedTask;
		}

        /// <summary>
        /// Executes the sign out.
        /// </summary>
        public void ExecuteSignOutCommand()
        {
            try
            {
                IsBusy = true;
                Dispose();
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task ExecuteRefreshCommand(object parameter)
        {          
            try
            {
                if ((!IsBusy) && (!IsRefreshing))
                {
                    IsBusy = true;
                    IsRefreshing = true;

                    this.RefreshData();
                }

                await Task.CompletedTask;
            }
            finally
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    IsBusy = false;
                    IsRefreshing = false;
                });
            }
        }

        protected virtual void RefreshData()
        {
        }

        public bool CanExecuteRefreshCommand(object parameter)
        {
            return (this.IsRefreshing == false && this.IsBusy == false);
        }

        /// <summary>
        /// Currents the connectivity changed.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        private void CurrentConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                this.IsConnected = (e.NetworkAccess == NetworkAccess.Internet);

                if (this.IsConnected == false)
                {
                    this.ErrorHandlerService.TriggerError(new Error(AppResources.ErrorInternetConnection, Errors.ERR0002_ErrorInternetConnection, true, true), this.DisplayError);
                }
                else
                {
                    this.ErrorHandlerService.DeleteError(Errors.ERR0002_ErrorInternetConnection, this.HideError);
                }
            });
        }

        /// <summary>
        /// Dispose the specified disposing.
        /// </summary>
        /// <returns>The dispose.</returns>
        /// <param name="disposing">If set to <c>true</c> disposing.</param>
		protected virtual void Dispose(bool disposing)
		{
			if (!Disposed)
			{
				if (disposing)
				{
					// Dispose managed resources.
					if (handle != null)
                    {
                        handle.Dispose();
                    }
				}

				// Dispose unmanaged managed resources.
				Disposed = true;
			}
		}

        /// <summary>
        /// Ons the disappearing.
        /// </summary>
        public virtual Task OnDisappearing()
        {
            return null;
        }

        /// <summary>
        /// Appearing this instance.
        /// </summary>
        public virtual Task OnAppearing()
        {
            return null;
        }

		public void DisplayError(Error error)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (error.Ident != null)
                {
                    string resource = AppResources.ResourceManager.GetString(error.Ident, App.CurrentCulture);

                    if (!string.IsNullOrEmpty(resource))
                    {
                        GlobalResources.Current.Message = resource;
                    }
                    else
                    {
                        GlobalResources.Current.Message = error.Message;
                    }
                }
                else
                {
                    GlobalResources.Current.Message = error.Message;
                }
            });
        }

        public void DisplayErrorDialogBox(Error error)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (error.Ident != null)
                {
                    string resource = AppResources.ResourceManager.GetString(error.Ident, App.CurrentCulture);

                    if (!string.IsNullOrEmpty(resource))
                    {
                        await this.DialogService.ShowError(resource, AppResources.Error, AppResources.Ok, null);
                    }
                    else
                    {
                        await this.DialogService.ShowError(error.Message, AppResources.Error, AppResources.Ok, null);
                    }
                }
                else
                {
                    await this .DialogService.ShowError(error.Message, AppResources.Error, AppResources.Ok, null);
                }
            });
        }

        public void HideError()
		{
            Device.BeginInvokeOnMainThread(() =>
            {
                GlobalResources.Current.Message = null;
            });
        }

        /// <summary>
        /// </summary>
		public void Dispose()
		{
			this.Dispose(true);

			Connectivity.ConnectivityChanged -= CurrentConnectivityChanged;

            GC.SuppressFinalize(this);
		}

        #endregion
    }
}
