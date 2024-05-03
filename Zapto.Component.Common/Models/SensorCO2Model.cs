namespace Zapto.Component.Common.Models
{
    public record SensorCO2Model : BaseModel
    {
        private int[] _thresholdCO2 = { 800, 1100 };
        public int Mode { get; set; }
        public int? CO2 { get; set; }
        public string? Description { get; set; } = string.Empty;
        public int[] ThresholdCO2
        {
            get { return _thresholdCO2; }

            set
            {
                _thresholdCO2 = value;
            }
        }
        public int? IsRunning { get; set; }
    }
}
