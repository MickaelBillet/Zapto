using Connect.Application;
using Connect.Mobile.Resources;
using Connect.Model;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Connect.Mobile.ViewModel
{
    public class SensorCellViewModel : BaseViewModel
    {
        #region Properties

        public ICommand SettingsSensorCommand
        {
            get; set;
        }

        public ConnectedObject ConnectedObject { get; set; }

        #endregion

        #region Services

        private IApplicationNotificationServices ApplicationNotificationServices { get; }

        #endregion

        #region Constructor

        public SensorCellViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.SettingsSensorCommand = new Command<ConnectedObject>(async (ConnectedObject item) => await ExecuteSettingsSensorCommand(item));

            this.ApplicationNotificationServices = serviceProvider.GetService<IApplicationNotificationServices>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// ExecuteSettingsSensorCommand
        /// </summary>
        /// <param name="item"></param>
        public async Task ExecuteSettingsSensorCommand(ConnectedObject item)
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                this.ConnectedObject = item;

                await this.NavigationService.NavigateToModalAsync<NotificationViewModel>(item, async (obj) => await this.ValidateCbNotification(obj), null);

                this.HandleError(Model.ErrorType.None, String.Empty);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);

                this.HandleError(Model.ErrorType.ErrorWebService, ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task ValidateCbNotification(object item)
        {
            try
            {
                if (this.IsConnected)
                {
                    Notification notification = (item as Notification).Clone<Notification>();

                    if (await this.ApplicationNotificationServices.AddUpdateNotification(this.ConnectedObject, notification) == false)
                    {
                        this.HandleError(Model.ErrorType.ErrorSoftware, AppResources.ErrorNotification);
                    }
                    else
                    {
                        this.HandleError(Model.ErrorType.None, String.Empty);
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

                this.HandleError(Model.ErrorType.ErrorWebService, ex.Message);
            }
            finally
            {
                await Task.FromResult(true);
            }
        }

        #endregion
    }
}
