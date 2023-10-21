
using System.Text.Json.Serialization;

namespace AirZapto.Model
{
    public class Sensor : ConnectDevice
    {
        private float? humidity = null;
        private float? temperature = null;
        private float? pressure = null;
        private int? _co2 = null;
        private string? period = string.Empty;
        private string channel = string.Empty;
        private int _mode = SensorMode.Initial;
        private int[] _thresholdCO2 = { 800, 1100 };
        private string? description = string.Empty;

        #region Property

        [JsonIgnore]
        public Sensor Self
        {
            get { return this; }
        }

        public int[] ThresholdCO2
        {
            get { return _thresholdCO2; }

            set
            {
                SetProperty(ref _thresholdCO2, value);

                OnPropertyChanged(nameof(Self));
            }
        }

        public int Status
        {
            get
            {
                int status = SensorStatus.None;

                if (Mode == SensorMode.Measure)
                {
                    if (CO2 < ThresholdCO2[0])
                    {
                        status = SensorStatus.Good;
                    }
                    else if (CO2 >= ThresholdCO2[0] && CO2 < ThresholdCO2[1])
                    {
                        status = SensorStatus.Average;
                    }
                    else if (CO2 > ThresholdCO2[1])
                    {
                        status = SensorStatus.Bad;
                    }
                }

                return status;
            }
        }

        public int Mode
        {
            get { return _mode; }

            set { SetProperty(ref _mode, value); }
        }

        public string IdSocket { get; set; } = string.Empty;

        public int Port { get; set; }

        public string IpAddress { get; set; } = string.Empty;

        public float OffSetHumidity { get; set; }

        public float OffSetTemperature { get; set; }

        public int? CO2
        {
            get { return _co2; }

            set
            {
                SetProperty(ref _co2, value);

                OnPropertyChanged(nameof(Self));
            }
        }

        public float? Humidity
        {
            get { return humidity; }

            set { SetProperty(ref humidity, value); }
        }

        public float? Temperature
        {
            get { return temperature; }

            set { SetProperty(ref temperature, value); }
        }

        public float? Pressure
        {
            get { return pressure; }

            set { SetProperty(ref pressure, value); }
        }

        public string? Period
        {
            get { return period; }

            set { SetProperty(ref period, value); }
        }

        public string? Description
        {
            get { return description; }

            set { SetProperty(ref description, value); }
        }

        public string Channel
        {
            get { return channel; }

            set { SetProperty(ref channel, value); }
        }

        #endregion

        #region Constructor

        public Sensor() : base()
        {
        }

        #endregion

        #region Method        

        public void Update(float temperature, float humidity, float pressure)
        {
            Temperature = temperature + OffSetTemperature;
            Humidity = humidity + OffSetHumidity;
            Pressure = pressure;
        }

        #endregion
    }
}
