using Connect.Mobile.Interfaces;
using Connect.Mobile.Resources;
using Connect.Model;
using Framework.Core.Base;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Command = Xamarin.Forms.Command;
using IErrorHandlerService = Connect.Mobile.Interfaces.IErrorHandlerService;

namespace Connect.Mobile.ViewModel
{
	public class BaseViewModel : ObservableObject, IDisposable, IPageLifeCylceEvents, IErrorHandlerService
    {
        // some fields that require cleanup
        private SafeHandle handle = null;

		bool isBusy = false;
        
		string title = string.Empty;

        Boolean isConnected = (Connectivity.NetworkAccess == NetworkAccess.Internet);

        private int errorType = Model.ErrorType.None;

        private string error = string.Empty;

        private bool isRefreshing;

        private bool isInitialized = false;

        private object item;

        private NotifyTaskCompletion loadingtask = null;

        #region Command

        public ICommand SignOutCommand { get; }

        public ICommand RefreshCommand { get; set; }

        #endregion

        #region Property

		private bool Disposed { get; set; } = false; // to detect redundant calls

        public string Error
        {
            get { return error; }
            protected set { SetProperty(ref error, value); }
        }

        public int ErrorType
        {
            get { return errorType; }

            protected set { SetProperty(ref errorType, value); }
        }

        public object Item
        {
            get { return item; }

            protected set { SetProperty(ref item, value); }
        }

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
                SetProperty<bool>(ref isRefreshing, value);
            }
        }

        public bool IsBusy
        {
            get { return isBusy; }

            set
            {

                SetProperty(ref isBusy, value);
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

        protected ILogger Logger { get; }

        public Boolean IsConnected
		{
			get { return isConnected; }
			private set { SetProperty(ref isConnected, value); }
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

            this.SignOutCommand = new Command(() => ExecuteSignOutCommand());
            this.RefreshCommand = new Command(() => ExecuteRefreshCommand(), CanExecuteRefreshCommand);

            Connectivity.ConnectivityChanged += CurrentConnectivityChanged;

            if (this.IsConnected == false)
            {
                this.Error = AppResources.NotConnected;
            }
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
        public virtual void Initialize(Object item, Func<Object, Task> validateCallback = null, Func<Object, Task> cancelCallback = null)
		{
            if (validateCallback != null)
            {
                this.ValidateCallback = validateCallback;
            }

            if (cancelCallback != null)
            {
                this.CancelCallback = cancelCallback;
            }

            this.Error = this.LoadingTask?.ErrorMessage;
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

        public void ExecuteRefreshCommand()
        {          
            try
            {
                if ((!IsBusy) && (!IsRefreshing))
                {
                    IsBusy = true;
                    IsRefreshing = true;

                    this.RefreshData();
                }
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

        public bool CanExecuteRefreshCommand()
        {
            return (!this.IsRefreshing && !this.IsBusy);
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
                    this.Error = AppResources.NotConnected;
                }
                else
                {
                    this.Error = null;
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

        /// <summary>
        /// Handles the error.
        /// </summary>
        /// <param name="type">Type.</param>
        /// <param name="msg">Message.</param>
		public void HandleError(int type, string msg)
        {
            this.ErrorType = type;

            if (!String.IsNullOrEmpty(msg))
            {
                String resource = AppResources.ResourceManager.GetString(msg, App.CurrentCulture);

                if (!String.IsNullOrEmpty(resource))
                {
                    this.Error = resource;
                }
                else
                {
                    this.Error = msg;
                }
            }
            else
            {
                this.Error = String.Empty;
            }

            if (this.ErrorType == Model.ErrorType.ErrorWebService)
            {
                this.IsConnected = false;

                #if !DEBUG
                this.Error = AppResources.ErrorServer;
                #endif
            }
            else
            {
                this.IsConnected = true;
            }
        }

        protected virtual void RefreshData() { }

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
