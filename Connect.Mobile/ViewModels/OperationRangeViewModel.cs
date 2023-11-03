using Connect.Mobile.Resources;
using Connect.Model;
using Framework.Core.Base;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Command = Xamarin.Forms.Command;

namespace Connect.Mobile.ViewModel
{
    public class OperationRangeViewModel : BaseViewModel
	{
        private DayOfWeek? day = null;

        private Boolean week = false;

        private Boolean weekEnd = false;

        private Boolean canSelectDay = true;

        private Boolean allDay = false;

        private TimeSpan startTime;

        private TimeSpan endTime;

        private Boolean canSelectTime = true;

        private ObservableCollection<OperationRange> operationRangeList = null;

        private Plug plug = null;

        private float? temperatureOrder = null;
        private float? humidityOrder = null;
        private int temperatureOrderIsEnabled = 0;
        private int humidityOrderIsEnabled = 0;

        #region Properties

        public Boolean Week
        {
            get { return week; }

            set
            {
                SetProperty<Boolean>(ref week, value);

                if ((this.WeekEnd == true) || (this.Week == true))
                {
                    this.CanSelectDay = false;
                }
                else
                {
                    this.CanSelectDay = true;
                }
            }
        }

        public Boolean WeekEnd
        {
            get { return weekEnd; }

            set 
            { 
                SetProperty<Boolean>(ref weekEnd, value); 

                if ((this.WeekEnd == true) || (this.Week == true))
                {
                    this.CanSelectDay = false;
                }
                else
                {
                    this.CanSelectDay = true;
                }
            }
        }

        public DayOfWeek? Day
        {
            get { return day; }

            set { SetProperty<DayOfWeek?>(ref day, value); }
        }

        public float? TemperatureOrder
        {
            get { return temperatureOrder; }

            set { SetProperty<float?>(ref temperatureOrder, value); }
        }

        public float? HumidityOrder
        {
            get { return humidityOrder; }

            set { SetProperty<float?>(ref humidityOrder, value); }
        }

        public int TemperatureOrderIsEnabled
        {
            get { return temperatureOrderIsEnabled; }

            set { SetProperty<int>(ref temperatureOrderIsEnabled, value); }
        }

        public int HumidityOrderIsEnabled
        {
            get { return humidityOrderIsEnabled; }

            set { SetProperty<int>(ref humidityOrderIsEnabled, value); }
        }

        public Boolean CanSelectDay
        {
            get { return canSelectDay; }

            set { SetProperty<Boolean>(ref canSelectDay, value); }
        }

        public TimeSpan StartTime
        {
            get { return startTime; }

            set { SetProperty<TimeSpan>(ref startTime, value); }
        }

        public TimeSpan EndTime
        {
            get { return endTime; }

            set { SetProperty<TimeSpan>(ref endTime, value); }
        }

        public Boolean AllDay
        {
            get { return allDay; }

            set 
            { 
                SetProperty<Boolean>(ref allDay, value); 

                if (this.AllDay)
                {
                    this.CanSelectTime = false;
                }
                else
                {
                    this.CanSelectTime = true;
                }
            }
        }

        public Boolean CanSelectTime
        {
            get { return canSelectTime; }

            set { SetProperty<Boolean>(ref canSelectTime, value); }
        }

        public ObservableCollection<OperationRange> OperationRangeList
        {
            get { return operationRangeList; }

            set { SetProperty<ObservableCollection<OperationRange>>(ref operationRangeList, value); }
        }

        public Plug Plug
        {
            get { return plug; }

            set { SetProperty<Plug>(ref plug, value); }
        }

        #endregion

        #region Commands

        public ICommand ConfirmCommand { get; }

        public ICommand CancelCommand { get; }

        public ICommand DeleteOpRangeCommand { get; }

        #endregion

        #region Constructor

        public OperationRangeViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
		{
			this.ConfirmCommand = new Command(async () => await ExecuteConfirmCommand());
            this.CancelCommand = new Command(async () => await ExecuteCancelCommand());
            this.DeleteOpRangeCommand = new Command(async () => await ExecuteDeleteOpRangeCommand());
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
            this.LoadingTask = new NotifyTaskCompletion(Task.Run(() =>
            {
                try
                {
                    if (item != null)
                    {
                        Tuple<ObservableCollection<OperationRange>, Plug> input = item as Tuple<ObservableCollection<OperationRange>, Plug>;

                        this.OperationRangeList = input.Item1;

                        this.Plug = input.Item2;

                        //Mode editing
                        if ((this.OperationRangeList != null) && (this.OperationRangeList.Count == 1))
                        {
                            this.Day = this.OperationRangeList[0].Day;

                            if (this.OperationRangeList[0].AllDay)
                            {
                                this.AllDay = true;

                                this.StartTime = this.OperationRangeList[0].StartTime;
                                this.EndTime = this.OperationRangeList[0].EndTime;
                            }
                            else
                            {
                                this.AllDay = false;

                                this.StartTime = this.OperationRangeList[0].StartTime;
                                this.EndTime = this.OperationRangeList[0].EndTime;
                            }

                            if (this.OperationRangeList[0].Condition != null)
                            {
                                this.TemperatureOrder = this.OperationRangeList[0].Condition.TemperatureOrder;
                                this.TemperatureOrderIsEnabled = this.OperationRangeList[0].Condition.TemperatureOrderIsEnabled;
                                this.HumidityOrder = this.OperationRangeList[0].Condition.HumidityOrder;
                                this.HumidityOrderIsEnabled = this.OperationRangeList[0].Condition.HumidityOrderIsEnabled;
                            }
                        }

                        base.Initialize(item, validateCallback, cancelCallback);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);

                    this.HandleError(Model.ErrorType.ErrorSoftware, ex.Message);
                }
            }));
		}

        /// <summary>
        /// Executes the confirm command.
        /// </summary>
        /// <returns>The confirm command.</returns>
        public async Task ExecuteConfirmCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            Boolean res = false;

            try
            {
                this.OperationRangeList.Clear();

                if ((this.Week) || (this.WeekEnd))
                {
                    if (this.Week)
                    {
                        foreach (DayOfWeek day in DayTime.Days)
                        {
                            if ((day == DayOfWeek.Monday) || (day == DayOfWeek.Tuesday) || (day == DayOfWeek.Wednesday) || (day == DayOfWeek.Thursday) || (day == DayOfWeek.Friday))
                            {
                                this.AddOperationRange(day);
                            }
                        }
                    }

                    if (this.WeekEnd)
                    {
                        foreach (DayOfWeek day in DayTime.Days)
                        {
                            if ((day == DayOfWeek.Saturday) || (day == DayOfWeek.Sunday))
                            {
                                this.AddOperationRange(day);
                            }
                        }
                    }

                    res = true;
                }
                else if (this.Day != null)
                {
                    this.AddOperationRange(this.Day.Value);

                    res = true;
                }
                else
                {
                    await this.DialogService.ShowError(AppResources.DayCannotBeNull, AppResources.Error, AppResources.OK, null);
                }

                if ((res == true) && (this.StartTime >= this.EndTime) && (!this.AllDay))
                {
                    await this.DialogService.ShowError(AppResources.StartEndTime, AppResources.Error, AppResources.OK, null);

                    res = false;
                }

                if ((this.ValidateCallback != null) && (res == true))
                {
                    await this.ValidateCallback(this.OperationRangeList);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);

                await this.DialogService.ShowError(ex.Message, AppResources.Error, AppResources.OK, null);
            }
            finally
            {
                if (res == true)
                {
                    await this.NavigationService.RemoveModalAsync();   
                }

                IsBusy = false;
            }
        }

        /// <summary>
        /// Executes the delete op range command.
        /// </summary>
        /// <returns>The delete op range command.</returns>
        public async Task ExecuteDeleteOpRangeCommand()
		{
			if (IsBusy)
				return;
            
			IsBusy = true;

			try
			{
                await this.DialogService.ShowMessage(AppResources.DeleteOperationRange, AppResources.Question + "?", AppResources.Delete, AppResources.Cancel, (async (Boolean res) =>
                {
                    if (res == true)
                    {
                        if (this.ValidateCallback != null)
                        {
                            this.OperationRangeList.Clear();

                            await this.ValidateCallback(this.OperationRangeList);
                        }

                        await this.NavigationService.RemoveModalAsync();
                    }
                }));
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
        /// ExecuteCancelCommand
        /// </summary>
        /// <returns></returns>
		public async Task ExecuteCancelCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                this.OperationRangeList.Clear();
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
        /// Adds the operation range.
        /// </summary>
        /// <returns><c>true</c>, if operation range was added, <c>false</c> otherwise.</returns>
        /// <param name="day">Day.</param>
        private void AddOperationRange(DayOfWeek day)
        {
            OperationRange operationRange = new OperationRange()
            {
                Id = Guid.NewGuid().ToString(),
            };

            Model.Condition condition = new Model.Condition()
            {
                Id = Guid.NewGuid().ToString(),
                OperationRangetId = operationRange.Id,
                HumidityOrder = null,
                TemperatureOrder = null,
            };

            operationRange.ConditionId = condition.Id;
            operationRange.Condition = condition;

            if (this.AllDay)
            {
                operationRange.StartTime = new TimeSpan(0, 0, 0);
                operationRange.EndTime = new TimeSpan(23, 59, 0);
                operationRange.Day = day;
                operationRange.AllDay = true;
            }
            else
            {
                operationRange.StartTime = this.StartTime;
                operationRange.EndTime = this.EndTime;
                operationRange.Day = day;
                operationRange.AllDay = false;
            }

            operationRange.Condition.TemperatureOrder = this.TemperatureOrder;
            operationRange.Condition.HumidityOrder = this.HumidityOrder;
            operationRange.Condition.TemperatureOrderIsEnabled = this.TemperatureOrderIsEnabled;
            operationRange.Condition.HumidityOrderIsEnabled = this.HumidityOrderIsEnabled;

            this.OperationRangeList.Add(operationRange);
        }

		#endregion
	}
}