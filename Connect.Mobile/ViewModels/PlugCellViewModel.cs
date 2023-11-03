using Connect.Application;
using Connect.Model;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Connect.Mobile.ViewModel
{
    public class PlugCellViewModel : BaseViewModel
    {
        #region Properties

        public ICommand SettingsPlugCommand
        {
            get; set;
        }

        public ICommand ProgrammingPlugCommand
        {
            get; set;
        }

        public ICommand OnOffPlugCommand
        {
            get; set;
        }

		#endregion

		#region Services

        private IApplicationPlugServices ApplicationPlugServices { get; }

		#endregion

		#region Constructor

		public PlugCellViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.ProgrammingPlugCommand = new Command<ConnectedObject>(async (ConnectedObject item) => await ExecuteProgrammingPlugCommand(item));
            this.OnOffPlugCommand = new Command<ConnectedObject>(async (ConnectedObject item) => await ExecuteOnOffPlugCommand(item));
            this.SettingsPlugCommand = new Command<ConnectedObject>(async (ConnectedObject item) => await ExecuteSettingsPlugCommand(item));

            this.ApplicationPlugServices = serviceProvider.GetService<IApplicationPlugServices>();
        }

        #endregion

        #region Method

        /// <summary>
        /// Initializes the async.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="item">Item.</param>
        public override void Initialize(object item, Func<object, Task> validateCallback = null, Func<object, Task> cancelCallback = null)
        {
            if (item != null)
            {
                base.Initialize(item, validateCallback, cancelCallback);
            }
        }

        /// <summary>
        /// Command Update Plug
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task ExecuteProgrammingPlugCommand(ConnectedObject item)
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                if (item != null)
                {
                    await this.ApplicationPlugServices.ChangeMode(item.Plug);
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        /// <summary>
        /// Executes the order plug command.
        /// </summary>
        /// <returns>The order plug command.</returns>
        /// <param name="item">Item.</param>
        public async Task ExecuteOnOffPlugCommand(ConnectedObject item)
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                if (item != null)
                {
                    await this.ApplicationPlugServices.SwitchOnOff(item.Plug);
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        /// <summary>
        /// ExecuteOpenSettingsCommand
        /// </summary>
        /// <param name="item"></param>
        public async Task ExecuteSettingsPlugCommand(ConnectedObject item)
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                await this.NavigationService.NavigateToModalAsync<SettingsViewModel>(item.Plug, null, null);

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

        #endregion
    }
}
