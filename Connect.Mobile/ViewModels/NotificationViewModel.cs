using Connect.Application;
using Connect.Mobile.Resources;
using Connect.Model;
using Framework.Core.Base;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Command = Xamarin.Forms.Command;

namespace Connect.Mobile.ViewModel
{
    public class NotificationViewModel : BaseViewModel
    {
        private INotificationProvider _notificationProvider;

        private Notification _notification;

        #region Properties

        public Notification Notification
        {
            get { return _notification; }

            set { SetProperty<Notification>(ref _notification, value); }
        }

        public INotificationProvider NotificationProvider
        {
            get { return _notificationProvider; }

            set { SetProperty<INotificationProvider>(ref _notificationProvider, value); }
        }

        public Action ClosePopuUp;

        public Action<bool, bool, bool, bool> ExpandPopup;

        #endregion

        #region Commands

        public ICommand CloseCommand { get; }

        public ICommand CreateNotificationCommand { get; }

        public ICommand DeleteAllNotificationCommand { get; }

        public ICommand CancelCommand { get; }

        public ICommand ConfirmCommand { get; }

        public ICommand EditNotificationCommand { get; }

        public ICommand DeleteNotificationCommand { get; }

        public ICommand OnOffNotificationCommand { get; }

		#endregion

		#region Services

        private IApplicationNotificationServices ApplicationNotificationServices { get; }

        #endregion

        #region Constructor

        public NotificationViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.CloseCommand = new Command(async () => await ExecuteCloseCommand());
            this.CreateNotificationCommand = new Command(async () => await ExecuteCreateNotificationCommand());
            this.DeleteAllNotificationCommand = new Command(async () => await ExecuteDeleteAllNotificationCommand());
            this.CancelCommand = new Command(async () => await ExecuteCancelCommand());
            this.ConfirmCommand = new Command(async () => await ExecuteConfirmCommand());
            this.EditNotificationCommand = new Command(async (obj) => await ExecuteEditNotificationCommand(obj));
            this.DeleteNotificationCommand = new Command(async (obj) => await ExecuteDeleteNotificationCommand(obj));
            this.OnOffNotificationCommand = new Command(async (obj) => await ExecuteOnOffNotificationCommand(obj));

            this.ApplicationNotificationServices = serviceProvider.GetService<IApplicationNotificationServices>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the async.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="item">Item.</param>
        public override void Initialize(Object item, Func<Object, Task> validateCallback = null, Func<Object, Task> cancelCallback = null)
        {
            if (item != null)
            {
                this.NotificationProvider = (item as INotificationProvider);               

                this.RefreshData();
            }

            base.Initialize(item, validateCallback, cancelCallback);
        }

        protected override void RefreshData()
        {
            this.LoadingTask = new NotifyTaskCompletion(Task.Run(async () =>
            {
                this.NotificationProvider.NotificationsList = await this.ApplicationNotificationServices.GetNotifications(this.NotificationProvider);
            }));
        }

        public async Task ExecuteCancelCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);

                await this.DialogService.ShowError(ex.Message, AppResources.Error, AppResources.OK, null);
            }
            finally
            {
                IsBusy = false;

                this.ClosePopuUp();
            }
        }

        public async Task ExecuteOnOffNotificationCommand(object parameter)
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                if (this.IsConnected)
                {
                    if (parameter != null)
                    {
                        this.Notification = parameter as Notification;

                        this.Notification.IsEnabled = !this.Notification.IsEnabled;

                        if (await this.ApplicationNotificationServices.AddUpdateNotification(this.NotificationProvider, this.Notification) == false)
                        {
                            this.HandleError(Model.ErrorType.ErrorSoftware, AppResources.ErrorNotification);
                        }
                        else
                        {
                            this.HandleError(Model.ErrorType.None, String.Empty);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);

                await this.DialogService.ShowError(ex.Message, AppResources.Error, AppResources.OK, null);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task ExecuteEditNotificationCommand(object parameter)
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                this.Notification = parameter as Notification;

                this.ExpandPopup((this.Notification.Sign == SignType.Lower),
                                    (this.Notification.Sign == SignType.Upper),
                                    (this.Notification.Parameter == ParameterType.Temperature),
                                    (this.Notification.Parameter == ParameterType.Humidity));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);

                await this.DialogService.ShowError(ex.Message, AppResources.Error, AppResources.OK, null);
            }
            finally
            {

                IsBusy = false;
            }
        }

        public async Task ExecuteDeleteNotificationCommand(object parameter)
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                if (this.IsConnected)
                {
                    if (parameter != null)
                    {
                        if (await this.ApplicationNotificationServices.DeleteNotification(this.NotificationProvider, parameter as Notification) == false)
                        {
                            this.HandleError(Model.ErrorType.ErrorSoftware, AppResources.ErrorNotification);
                        }
                        else
                        {
                            this.HandleError(Model.ErrorType.None, String.Empty);
                        }
                    }
                }
                else
                {
                    this.HandleError(Model.ErrorType.Warning, AppResources.NotConnected);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);

                await this.DialogService.ShowError(ex.Message, AppResources.Error, AppResources.OK, null);
            }
            finally
            {

                IsBusy = false;
            }
        }

        public async Task ExecuteConfirmCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                await this.ValidateCallback(this.Notification);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);

                await this.DialogService.ShowError(ex.Message, AppResources.Error, AppResources.OK, null);
            }
            finally
            {

                IsBusy = false;

                this.Notification = null;

                this.ClosePopuUp();
            }
        }

        public async Task ExecuteCloseCommand()
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

                await this.DialogService.ShowError(ex.Message, AppResources.Error, AppResources.OK, null);
            }
            finally
            {
                await this.NavigationService.RemoveModalAsync();

                IsBusy = false;
            }
        }

        public async Task ExecuteDeleteAllNotificationCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                if (this.IsConnected)
                {
                    await this.DialogService.ShowMessage(AppResources.DeleteNotifications, AppResources.Question + "?", AppResources.Delete, AppResources.Cancel, (Boolean res) =>
                    {
                        if (res == true)
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                try
                                {
                                    if (await this.ApplicationNotificationServices.DeleteNotifications(this.NotificationProvider) == false)
                                    {
                                        this.HandleError(Model.ErrorType.ErrorSoftware, AppResources.ErrorNotification);
                                    }
                                    else
                                    {
                                        this.HandleError(Model.ErrorType.None, String.Empty);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    await this.DialogService.ShowError(ex.Message, AppResources.Error, AppResources.OK, null);
                                }
                            });
                        }
                    });
                }
                else
                {
                    this.HandleError(Model.ErrorType.Warning, AppResources.NotConnected);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);

                await this.DialogService.ShowError(ex.Message, AppResources.Error, AppResources.OK, null);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task ExecuteCreateNotificationCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                this.Notification = new Notification()
                {
                    Sign = SignType.None,
                    Parameter = ParameterType.None,
                };

                this.ExpandPopup(false, false, false, false);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);

                await this.DialogService.ShowError(ex.Message, AppResources.Error, AppResources.OK, null);
            }
            finally
            {
                IsBusy = false;
            }
        }

        #endregion
    }
}
