using Framework.Core.Base;
using Serilog;
using System;
using System.Linq;
using System.Text.Json.Serialization;

namespace Connect.Model
{
    public class Plug : ConnectDevice
    {
        private int _status = Model.Status.OFF;
        private int _mode = Model.Mode.Manual;
        private int _onoff = 0;
        private int _order = 0;
        private Condition? _condition = null;
        private Program? _program = null;
        private int _conditionType = 0;
        private Configuration? _configuration = null;

        #region Property

        public string ConfigurationId { get; set; } = string.Empty;

        public string ProgramId { get; set; } = string.Empty;

        public string ConditionId { get; set; } = string.Empty;

        public DateTime? LastCommandDateTime { get; set; } = null;

        bool LastCommandSent { get; set; } = false;

        public Configuration? Configuration
        {
            get { return _configuration; }

            set { SetProperty<Configuration?>(ref _configuration, value); }
        }

        public Program? Program
        {
            get { return _program; }

            set { SetProperty<Program?>(ref _program, value); }
        }

        public Condition? Condition
        {
            get { return _condition; }

            set { SetProperty<Condition?>(ref _condition, value); }
        }

        public int Status
        {
            get { return _status; }

            set
            {
                SetProperty<int>(ref _status, value);

                this.OnPropertyChanged(nameof(Self));
            }
        }

        public int OnOff
        {
            get { return _onoff; }

            set
            {
                SetProperty<int>(ref _onoff, value);
            }
        }

        public int Order
        {
            get { return _order; }

            set
            {
                SetProperty<int>(ref _order, value);

                this.OnPropertyChanged(nameof(Self));
            }
        }

        public int Mode
        {
            get { return _mode; }

            set
            {
                SetProperty<int>(ref _mode, value);
            }
        }

        public int ConditionType
        {
            get { return _conditionType; }

            set { SetProperty<int>(ref _conditionType, value); }
        }

        [Newtonsoft.Json.JsonIgnore]
        [JsonIgnore]
        public Plug Self
        {
            get { return this; }
        }

        #endregion

        #region Constructor

        public Plug() : base()
        {      

        }

        #endregion

        #region Method

        /// <summary>
        /// Update the order of the plug
        /// </summary>
        /// <param name="humidity"></param>
        /// <param name="temperature"></param>
        /// <returns></returns>
        public void UpdateOrder(double? humidity, double? temperature)
        {
            if (this.OnOff == 1)
            {
                //Manual mode
                if (this.Mode == Model.Mode.Manual)
                {
                    if (this.Condition != null)
                    {
                        if (this.Condition.TemperatureOrderIsEnabled == 1)
                        {
                            this.Order = this.UpdateOrderFromTemperature(this.Condition, temperature);
                        }
                        else if (this.Condition.HumidityOrderIsEnabled == 1) 
                        {
                            this.Order = this.UpdateOrderFromHumidity(this.Condition, humidity);
                        }
                        else
                        {
                            this.Order = Model.Order.On;
                        }
                    }
                    else
                    {
                        this.Order = Model.Order.On;
                    }
                }
                //Programming mode
                else if (this.Mode == Model.Mode.Programing)
                {
                    if ((this.Program?.OperationRangeList != null) && (this.Program?.OperationRangeList.Any() == true))
                    {
                        foreach (OperationRange range in this.Program.OperationRangeList)
                        {
                            if ((range.Day == Clock.Now.DayOfWeek) //Check Day
                                && (range.StartTime <= Clock.Now.TimeOfDay) && (range.EndTime >= Clock.Now.TimeOfDay))  //Check Time
                            {
                                if (this.Condition != null)
                                {
                                    if (this.Condition.TemperatureOrderIsEnabled == 1)
                                    {
                                        this.Order = this.UpdateOrderFromTemperature(this.Condition, temperature);
                                    }
                                    else if (this.Condition.HumidityOrderIsEnabled == 1)
                                    {
                                        this.Order = this.UpdateOrderFromHumidity(this.Condition, humidity);
                                    }
                                    else
                                    {
                                        this.Order = Model.Order.On;
                                    }
                                }
                                else
                                {
                                    this.Order = Model.Order.On;
                                }
                                break;
                            }
                            else
                            {
                                this.Order = Model.Order.Off;
                            }
                        }
                    }
                    else
                    {
                        this.Order = Model.Order.Off;
                    }
                }
            }
            else
            {
                this.Order = Model.Order.Off;
            }
        }

        private int UpdateOrderFromHumidity(Condition condition, double? humidity)
        {
            int order = 1;

            if ((humidity != null) && (condition.HumidityOrder != null))
            {
                if (humidity > condition.HumidityOrder)
                {
                    order = Model.Order.On;
                }
                else
                {
                    order = Model.Order.Off;
                }
            }

            return order;
        }

        private int UpdateOrderFromTemperature(Condition condition, double? temperature) 
        {
            int order = Model.Order.On;

            if ((temperature != null) && (condition.TemperatureOrder != null))
            {
                if (temperature > condition.TemperatureOrder)
                {
                    order = Model.Order.On;
                }
                else
                {
                    order = Model.Order.Off;
                }
            }

            return order;
        }
        /// <summary>
        /// Compute the working duration
        /// </summary>
        public void ComputeWorkingDuration()
        {
            if (this.LastDateTimeOn != null)
            {
                this.WorkingDuration = this.WorkingDuration + DateTime.Now.Subtract(this.LastDateTimeOn.Value).TotalSeconds;
            }

            if (this.Status == Model.Status.ON)
            {
                this.LastDateTimeOn = DateTime.Now;
            }
            else
            {
                this.LastDateTimeOn = null;
            }
        }
        
        public void UpdateStatus(string status)
		{
            if ((this.Type & DeviceType.Outlet) == DeviceType.Outlet)
            {
                this.Status = (status.Equals(Command.PLUG_ON) ? Model.Status.ON : Model.Status.OFF);
            }
            else if ((this.Type & DeviceType.Module) == DeviceType.Module)
            {
                //reverse operation for a module
                this.Status = (status.Equals(Command.PLUG_OFF) ? Model.Status.ON : Model.Status.OFF);
            }

            //Update the working duration
            this.ComputeWorkingDuration();

            Log.Information("UpdateStatusPlug " + "Id : " + this.Id);
            Log.Information("UpdateStatusPlug " + "Status : " + this.Status);
            Log.Information("UpdateStatusPlug " + "Working duration : " + this.WorkingDuration);
        }

        public static int GetCommand(int status, int mode)
        {
            int command;
            if ((status == Model.Status.ON) && (mode == Model.Mode.Manual))
            {
                command = CommandType.Manual;
            }
            else if ((status == Model.Status.ON) && (status == Model.Mode.Programing))
            {
                command = CommandType.Programing;
            }
            else
            {
                command = CommandType.Off;
            }
            return command;
        }

        public static int GetStatus(int status, int order)
        {
            int result = Model.Status.OFF;
            if ((status == Model.Status.ON) && (order == Model.Order.On))
            {
                result = Model.Status.ON;
            }
            else if ((status == Model.Status.ON) && (order == Model.Order.Off))
            {
                result = Model.Status.OffON;
            }
            else if ((status == Model.Status.OFF) && (order == Model.Order.On))
            {
                result = Model.Status.OnOFF;
            }
            else if ((status == Model.Status.OFF) && (order == Model.Order.Off))
            {
                result = Model.Status.OFF;
            }
            return result;
        }

        #endregion
    }
}
