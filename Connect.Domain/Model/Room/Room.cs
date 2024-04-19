using Framework.Core.Base;
using Framework.Core.Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Connect.Model
{
    public class Room : Item, INotificationProvider
    {
        private string name = string.Empty;
        private string description = string.Empty;
        private ICollection<ConnectedObject> connectedObjects = new ObservableCollection<ConnectedObject>();
        private ICollection<Sensor> sensors = new ObservableCollection<Sensor>();
        private ICollection<Notification> notifications = new ObservableCollection<Notification>();
        private double? humidity = null;
        private double? temperature = null;
        private double? pressure = null;
        private int deviceType = 0;
        private byte statusSensors = RunningStatus.None;

        #region Property

        public string LocationId { get; set; } = string.Empty;

        public short Type { get; set; }

        public byte StatusSensors
        {
            get { return statusSensors; }
            set { SetProperty(ref statusSensors, value); }
        }

        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

        public string Description
        {
            get { return description; }
            set { SetProperty(ref description, value); }
        }

        /// <summary>
        /// Connected objects list of the room (heater, ventilation, refrigerator...)
        /// </summary>
        public ICollection<ConnectedObject> ConnectedObjectsList
        {
            get { return connectedObjects; }

            set { SetProperty(ref connectedObjects, value); }
        }

        public ICollection<Notification> NotificationsList
        {
            get { return notifications; }

            set { SetProperty(ref notifications, value); }
        }

        /// <summary>
        /// Sensors list of the room (Temperature, humidity...)
        /// </summary>
        public ICollection<Sensor> SensorsList
        {
            get { return sensors; }

            set { SetProperty(ref sensors, value); }
        }

        public double? Humidity
        {
            get { return humidity; }

            set { SetProperty(ref humidity, value); }
        }

        public double? Temperature
        {
            get { return temperature; }

            set { SetProperty(ref temperature, value); }
        }

        public double? Pressure
        {
            get { return pressure; }

            set { SetProperty(ref pressure, value); }
        }

        /// <summary>
        /// Allows to know the types of device present
        /// </summary>
        public int DeviceType
        {
            get { return deviceType; }

            set { SetProperty(ref deviceType, value); }
        }

        #endregion

        #region Constructor

        public Room() : base()
        {

        }

        #endregion

        #region Methods

        /// <summary>
        /// Computes the data of the room.
        /// </summary>
        public void ComputeData(int validityPeriod)
        {
            double temperatureSensor = 0;
            double sumTemperature = 0;
            double humidityeSensor = 0;
            double sumHumidity = 0;
            double pressureSensor = 0;
            double sumPressure = 0;

            foreach (Sensor sensor in this.SensorsList)
            {
                if (sensor.Date + new TimeSpan(0, validityPeriod, 0) > Clock.Now)
                {
                    if (Model.DeviceType.Sensor_Temperature == (sensor.Type & Model.DeviceType.Sensor_Temperature))
                    {
                        if (sensor.Temperature != null)
                        {
                            temperatureSensor++;
                            sumTemperature = sumTemperature + sensor.Temperature.Value;
                        }
                    }

                    if (Model.DeviceType.Sensor_Humidity == (sensor.Type & Model.DeviceType.Sensor_Humidity))
                    {
                        if (sensor.Humidity != null)
                        {
                            humidityeSensor++;
                            sumHumidity = sumHumidity + sensor.Humidity.Value;
                        }
                    }

                    if (Model.DeviceType.Sensor_Pressure == (sensor.Type & Model.DeviceType.Sensor_Pressure))
                    {
                        if (sensor.Pressure != null)
                        {
                            pressureSensor++;
                            sumPressure = sumPressure + sensor.Pressure.Value;
                        }
                    }
                }
            }

            if (temperatureSensor > 0)
            {
                this.Temperature = sumTemperature / temperatureSensor;
            }

            if (humidityeSensor > 0)
            {
                this.Humidity = sumHumidity / humidityeSensor;
            }

            if (pressureSensor > 0)
            {
                this.Pressure = sumPressure / pressureSensor;
            }
        }

        public void SetStatusSensors()
        {
            int sensorsDownCount = 0;
            int sensorsUpCount = 0;

            if (this.SensorsList != null && this.SensorsList.Any())
            {
                List<Sensor> sensors = this.SensorsList.Where(sensor => ((sensor.Type & Model.DeviceType.Sensor_Water_Leak) != Model.DeviceType.Sensor_Water_Leak)).ToList();
                foreach (Sensor sensor in sensors)
                {
                    if (sensor.IsRunning == RunningStatus.UnHealthy)
                    {
                        sensorsDownCount++;
                    }
                    else
                    {
                        sensorsUpCount++;
                    }
                }

                if (sensors.Count() == sensorsDownCount)
                {
                    this.StatusSensors = RunningStatus.UnHealthy;
                }
                else if (sensors.Count() == sensorsUpCount)
                {
                    this.StatusSensors = RunningStatus.Healthy;
                }
                else
                {
                    this.StatusSensors = RunningStatus.Degraded;
                }
            }
            else
            {
                this.StatusSensors = RunningStatus.None;
            }
        }

        public static int CompareByDescription(Room x, Room y)
        {
            if (x == null)
            {
                if (y == null)
                {
                    // If x is null and y is null, they're
                    // equal. 
                    return 0;
                }
                else
                {
                    // If x is null and y is not null, y
                    // is greater. 
                    return -1;
                }
            }
            else
            {
                // If x is not null...
                //
                if (y == null)
                // ...and y is null, x is greater.
                {
                    return 1;
                }
                else
                {
                    // ...and y is not null, compare the 
                    // lengths of the two strings.
                    //
                    return x.Description.CompareTo(y.Description);
                }
            }
        }

        #endregion
    }
}
