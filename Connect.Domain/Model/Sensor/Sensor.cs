using Framework.Core.Base;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Connect.Model
{
    public class Sensor : ConnectDevice
    {
        private double? humidity = null;
        private double? temperature = null;
        private double? pressure = null;
        private string channel = string.Empty;
        private int leakDetected = 0;
        private string parameter = string.Empty;
        private byte isRunning = RunningStatus.Healthy;

        #region Property

        [Newtonsoft.Json.JsonIgnore]
        [JsonIgnore]
        public string IpAddress { get; set; } = string.Empty;

        [Newtonsoft.Json.JsonIgnore]
        [JsonIgnore]
        public float OffSetHumidity { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        [JsonIgnore]
        public float OffSetTemperature { get; set; }

        public int LeakDetected
        {
            get { return leakDetected; }

            set { SetProperty<int>(ref leakDetected, value); }
        }

		public double? Humidity
        {
            get { return humidity; }

            set { SetProperty<double?>(ref humidity, value); }
        }

        public byte IsRunning
        {
            get { return isRunning; }

            set { SetProperty<byte>(ref isRunning, value); }
        }

        public double? Temperature
        {
            get { return temperature; }

            set { SetProperty<double?>(ref temperature, value); }
        }

        public double? Pressure
        {
            get { return pressure; }

            set { SetProperty<double?>(ref pressure, value); }
        }

        public string Parameter
        {
            get { return parameter; }

            set { SetProperty<string>(ref parameter, value); }
        }

        public string Channel
        {
            get { return channel; }

            set { SetProperty<string>(ref channel, value); }
        }

        #endregion

        #region Constructor

        public Sensor() : base()
        {
        }

        #endregion

        #region Method        

        public void ProcessData(float temperature, float humidity, float pressure)
        {
            this.Humidity = humidity + this.OffSetHumidity;
            this.Temperature = temperature + this.OffSetTemperature;
            this.Pressure = pressure;
            this.Date = Clock.Now;
        }       

        public string? GetSensorConfiguration()
        {
            SensorConfiguration configuration = new SensorConfiguration()
            {
                Type = this.Name,
                Channel = this.Channel,
                Parameter = this.Parameter,
            };

            string? json = JsonSerializer.Serialize(configuration);
            return json;
        }

        #endregion
    }
}
