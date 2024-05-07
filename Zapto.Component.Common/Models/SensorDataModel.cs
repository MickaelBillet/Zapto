namespace Zapto.Component.Common.Models
{
	public sealed record SensorDataModel : ObjectConnectedModel
	{
        private string? _temperature;

        public string? Temperature
        {
            get
            {
                return string.Format("{0} °C", _temperature);
            }

            set
            {
                _temperature = value;
            }
        }

        public byte IsRunning { get; set; }
    }
}
