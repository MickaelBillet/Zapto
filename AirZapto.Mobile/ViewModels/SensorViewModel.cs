using AirZapto.Application;
using AirZapto.Mobile.Resources;
using AirZapto.Model;
using Framework.Core.Base;
using Framework.Infrastructure.Services;
using Microcharts;
using Microsoft.Extensions.DependencyInjection;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AirZapto.ViewModel
{
    public class SensorViewModel : BaseViewModel
	{
		private Sensor _sensor = null;
        private Chart _sensorDataLineChart = null;
        private int _maxCO2 = 0;
        private int _minCO2 = 0;
        private int _duration = 60;
        private string _startDate = string.Empty;
        private string _endDate = string.Empty;

        #region Properties

        public Sensor Sensor
        {
            get { return _sensor; }

            set
            {
                SetProperty<Sensor>(ref _sensor, value);               
            }
        }

        public int MaxCO2
        {
            get { return _maxCO2; }

            set
            {
                SetProperty<int>(ref _maxCO2, value);
            }
        }

        public int MinCO2
        {
            get { return _minCO2; }

            set
            {
                SetProperty<int>(ref _minCO2, value);
            }
        }

        public Chart SensorDataLineChart
        {
            get { return _sensorDataLineChart; }

            set
            {
                SetProperty<Chart>(ref _sensorDataLineChart, value);
            }
        }

        public int Duration
        {
            get { return _duration; }

            set
            {
                SetProperty<int>(ref _duration, value);
            }
        }

        public string StartDate
        {
            get { return _startDate; }

            set
            {
                SetProperty<string>(ref _startDate, value);
            }
        }

        public string EndDate
        {
            get { return _endDate; }

            set
            {
                SetProperty<string>(ref _endDate, value);
            }
        }

        #endregion

        #region Commands 

        public ICommand SetDurationCommand { get; set; }

        #endregion

        #region Services

        private IApplicationSensorDataServices ApplicationSensorDataServices { get; }

        #endregion

        #region Constructor

        public SensorViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
		{
            this.ApplicationSensorDataServices = serviceProvider.GetService<IApplicationSensorDataServices>();
            this.SetDurationCommand = new Command<object>((obj) => ExecuteSetDurationCommand(obj));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the async.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="item">Item.</param>
        public override async Task Initialize(Object item, Func<Object, Task> validateCallback = null, Func<Object, Task> cancelCallback = null)
        {
            try
            {
                if (item != null)
                {
                    this.Sensor = item as Sensor;
                    this.RefreshData();
                    await base.Initialize(null, validateCallback, cancelCallback);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                this.ErrorHandlerService.TriggerError(new Error(ex.Message, null, true, false), this.DisplayError);
            }
            finally
            {
            }
        }        

        public void ExecuteSetDurationCommand(object parameter)
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                this.Duration = int.Parse((string)parameter);
                this.RefreshData();
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

        public bool CanSetDurationCommand(object parameter)
        {
            return true;
        }

        protected override void RefreshData()
        {
            this.LoadingTask = new NotifyTaskCompletion(Task.Run(async () =>
            {
                try
                {
                    IEnumerable<AirZaptoData> data = await this.ApplicationSensorDataServices.GetSensorDataAsync(this.Sensor.Id, this.Duration).ConfigureAwait(true);

                    if ((data?.Count() > 0))
                    {
                        IEnumerable<ChartEntry> entries = data.Where((arg) => arg.SensorId == this.Sensor.Id)
                                                                            .Select((arg) => new ChartEntry(arg.CO2.Value)
                                                                            {
                                                                                Color = this.SetColor(arg.CO2.Value),
                                                                            });

                        this.SensorDataLineChart = new LineChart()
                        {
                            Entries = entries,
                            LineMode = LineMode.Straight,
                            LineSize = 7,
                            PointMode = PointMode.None,
                            BackgroundColor = SKColor.Empty,
                            MinValue = entries.Min(x => x.Value) - 100,
                            MaxValue = entries.Max(x => x.Value) - 100,
                            EnableYFadeOutGradient = true,                            
                        };

                        this.MinCO2 = data.Min((x) => x.CO2.Value);
                        this.MaxCO2 = data.Max((x) => x.CO2.Value);
                        this.StartDate = data.Min((x) => x.Date).ToString("g");
                        this.EndDate = data.Max((x) => x.Date).ToString("g");

                        this.ErrorHandlerService.CleanError(this.HideError);
                    }
                    else
					{
                        this.SensorDataLineChart = new LineChart()
                        {
                            Entries = null,
                            LineMode = LineMode.Straight,
                            LineSize = 7,
                            PointMode = PointMode.None,
                            BackgroundColor = SKColor.Empty,
                            EnableYFadeOutGradient = true,
                        };

                        this.StartDate = string.Empty;
                        this.EndDate = string.Empty;

                        this.ErrorHandlerService.CleanError(this.HideError);
                    }
                }
                catch (InternetException)
                {
                    this.IsConnected = false;
                }
                catch (Exception)
                {
                    this.ErrorHandlerService.TriggerError(new Error(AppResources.ErrorWebService, Errors.ERR0001_ErrorWebService, true, true), this.DisplayError);
                }
            }));
        }

        private SKColor SetColor(int value)
        {
            SKColor color = SKColor.Empty;

            if (value < (this.Sensor.ThresholdCO2[0] - 200)) //600
            {
                color = SKColor.Parse("#37A921");
            }
            else if ((value >= (this.Sensor.ThresholdCO2[0] - 200)) && (value < (this.Sensor.ThresholdCO2[0]))) //600 - 800
            {
                color = SKColor.Parse("#8AE379");
            }
            else if ((value >= (this.Sensor.ThresholdCO2[0])) && (value < (this.Sensor.ThresholdCO2[0] + 100))) //800 - 900
            {
                color = SKColor.Parse("#FBFF00");
            }
            else if ((value >= (this.Sensor.ThresholdCO2[0] + 100)) && (value < (this.Sensor.ThresholdCO2[1] - 100))) //900 - 1000
            {
                color = SKColor.Parse("#FFD500");
            }
            else if ((value >= (this.Sensor.ThresholdCO2[1] - 100)) && (value < (this.Sensor.ThresholdCO2[1]))) //1000 - 1100
            {
                color = SKColor.Parse("#FF9B00");
            }
            else if ((value >= (this.Sensor.ThresholdCO2[1])) && (value < (this.Sensor.ThresholdCO2[1] + 400))) //1100 - 1400
            {
                color = SKColor.Parse("#FF7B34");
            }
            else
            {
                color = SKColor.Parse("#CE2222");
            }

            return color;
        }

        #endregion
    }
}
