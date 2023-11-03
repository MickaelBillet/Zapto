using Connect.Application;
using Connect.Mobile.Resources;
using Connect.Model;
using Framework.Core.Base;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Command = Xamarin.Forms.Command;

namespace Connect.Mobile.ViewModel
{
    public class SettingsViewModel : BaseViewModel
	{
        private Plug plug = null;

        #region Properties

        public Plug Plug
        {
            get { return plug; }
            set { SetProperty(ref plug, value); }
        }

        private OperationRange EditedOperationRange
        {
            get; set;
        }

        #endregion

        #region Commands

        public ICommand CreateOpRangeCommand { get; }

        public ICommand EditOpRangeCommand { get; }

        public ICommand DeleteAllOpRangeCommand { get; }

        public ICommand CloseCommand { get; }

		#endregion

		#region Services

        private IApplicationProgramServices ApplicationProgramServices { get; }

        private IApplicationConditionServices ApplicationConditionServices { get; }

		#endregion

		#region Constructor

		public SettingsViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
		{
            this.CreateOpRangeCommand = new Command(async () => await ExecuteCreateOpRangeCommand());
            this.EditOpRangeCommand = new Command<OperationRange>(async (OperationRange arg) => await ExecuteEditOpRangeCommand(arg));
            this.DeleteAllOpRangeCommand = new Command(async () => await ExecuteDeleteAllOpRangeCommand());
            this.CloseCommand = new Command(async () => await ExecuteCloseCommand());
            this.ApplicationProgramServices = serviceProvider.GetService<IApplicationProgramServices>();
            this.ApplicationConditionServices = serviceProvider.GetService<IApplicationConditionServices>();
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
                if (item != null)
                {
                    this.Plug = item as Plug;

                    if (this.Plug.Program != null)
                    {
                        this.Plug.Program.OperationRangeList = await this.ApplicationProgramServices.GetOperationsRanges(this.Plug.ProgramId);
                    }
                }

                base.Initialize(item, validateCallback, cancelCallback);
            }));
		}

        /// <summary>
        /// Delete all the operation ranges
        /// </summary>
        /// <returns></returns>
        public async Task ExecuteDeleteAllOpRangeCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                if (this.IsConnected)
                {
                    await this.DialogService.ShowMessage(AppResources.DeleteOperationRanges, AppResources.Question + "?", AppResources.Delete, AppResources.Cancel, async (Boolean res) =>
                    {
                        if (res == true)
                        {
                            try
                            {
                                if (await this.ApplicationProgramServices.DeleteOperationRanges(this.Plug.Program) == false)
                                {
                                    this.HandleError(Model.ErrorType.ErrorSoftware, AppResources.ErrorOperationRange);
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

        /// <summary>
        /// Executes the edit op range command.
        /// </summary>
        /// <returns>The edit op range command.</returns>
        /// <param name="operationRange">Operation range.</param>
        public async Task ExecuteEditOpRangeCommand(OperationRange operationRange)
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                this.EditedOperationRange = operationRange;

                if (this.EditedOperationRange != null)
                {
                    OperationRange clone = this.EditedOperationRange.Clone<OperationRange>();

                    ObservableCollection<OperationRange> collection = new ObservableCollection<OperationRange>
                    {
                        clone,
                    };

                    Tuple<ObservableCollection<OperationRange>, Plug> input = new Tuple<ObservableCollection<OperationRange>, Plug>(collection, this.Plug);

                    await this.NavigationService.NavigateToModalAsync<OperationRangeViewModel>(input, async (obj) => await this.AddOperationRangeAsync(obj), null);
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

        /// <summary>
        /// Create an OperationRange
        /// </summary>
        /// <returns></returns>
        public async Task ExecuteCreateOpRangeCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                this.EditedOperationRange = null;

                ObservableCollection<OperationRange> collection = new ObservableCollection<OperationRange>();

                Tuple<ObservableCollection<OperationRange>, Plug> input = new Tuple<ObservableCollection<OperationRange>, Plug>(collection, this.Plug);

                await this.NavigationService.NavigateToModalAsync<OperationRangeViewModel>(input, async (obj) => await this.AddOperationRangeAsync(obj), null);
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

        /// <summary>
        /// Adds the operation range async.
        /// </summary>
        /// <returns>The operation range async.</returns>
        /// <param name="operationRanges">Operation ranges.</param>
        private async Task AddOperationRangeAsync(Object operationRanges)
        {
            try
            {
                if (this.IsConnected)
                {
                    if (this.EditedOperationRange != null)
                    {
                        await this.ApplicationProgramServices.DeleteOperationRange(this.Plug.Program, this.EditedOperationRange);

                        this.EditedOperationRange = null;
                    }

                    ObservableCollection<OperationRange> addedList = operationRanges as ObservableCollection<OperationRange>;

                    if (addedList != null)
                    {
                        foreach (OperationRange operationRange in addedList)
                        {
                            operationRange.ProgramId = this.Plug.ProgramId;

                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                try
                                {
                                    //Add in memory Op after check overlapping
                                    (bool hasError, bool hasOverLapping) = await this.ApplicationProgramServices.AddOperationRange(this.Plug.Program, operationRange);


                                    if (hasError == true)
                                    {
                                        this.HandleError(Model.ErrorType.ErrorSoftware, AppResources.ErrorOperationRange);
                                    }
                                    else
                                    {
                                        if (hasOverLapping == true)
                                        {
                                            await this.DialogService.ShowMessage(AppResources.ErrorOperationRange + " (" + operationRange.Day + " - "
                                                                                                                        + operationRange.StartTime.ToString("c") + " - "
                                                                                                                        + operationRange.EndTime.ToString("c") + ") "
                                                                                                                        + AppResources.OverlapingRange,
                                                                                                                        AppResources.Error, AppResources.OK, null);
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Debug.WriteLine(ex);

                                    this.HandleError(Model.ErrorType.ErrorSoftware, ex.Message);
                                }
                            });
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

                this.HandleError(Model.ErrorType.ErrorWebService, ex.Message);
            }
        }

        public async Task ExecuteCloseCommand()
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
                await this.NavigationService.RemoveModalAsync();

                IsBusy = false;
            }
        }

        /// <summary>
        /// On the disappearing.
        /// </summary>
        public override async Task OnDisappearing()
        {
            try
            { 
                bool? result = await this.ApplicationConditionServices.UpdateCondition(this.Plug.Condition);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);

                this.HandleError(Model.ErrorType.ErrorWebService, ex.Message);
            }
        }

        #endregion
    }
}