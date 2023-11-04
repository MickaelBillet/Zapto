using System.Text.Json.Serialization;
using Framework.Core.Domain;

namespace Connect.Model
{
    public class Condition : Item
    {
        private float? humidityOrder = null;
        private float? temperatureOrder = null;
        private int temperatureOrderIsEnabled = 0;
        private int humidityOrderIsEnabled = 0;

        #region Property

        public string? OperationRangetId { get; set; }

        public float? HumidityOrder
        {
            get { return humidityOrder; }

            set
            {
                SetProperty<float?>(ref humidityOrder, value);

                this.OnPropertyChanged(nameof(Self));
            }
        }

        public float? TemperatureOrder
        {
            get { return temperatureOrder; }

            set
            {
                SetProperty<float?>(ref temperatureOrder, value);

                this.OnPropertyChanged(nameof(Self));
            }
        }

        public int TemperatureOrderIsEnabled
        {
            get { return temperatureOrderIsEnabled; }

            set
            {
                SetProperty<int>(ref temperatureOrderIsEnabled, value);

                this.OnPropertyChanged(nameof(Self));
            }
        }

        public int HumidityOrderIsEnabled
        {
            get { return humidityOrderIsEnabled; }

            set
            {
                SetProperty<int>(ref humidityOrderIsEnabled, value);

                this.OnPropertyChanged(nameof(Self));
            }
        }

        [Newtonsoft.Json.JsonIgnore]
        [JsonIgnore]
        public Condition Self
        {
            get { return this; }
        }

        #endregion

        #region Constructor

        public Condition(): base() { }

        #endregion
    }
}
