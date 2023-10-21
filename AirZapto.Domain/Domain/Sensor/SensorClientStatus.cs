namespace AirZapto.Model
{
    public class SensorClientStatus
    {
        #region Property

        public static string Name = "SensorStatus";

        public float? Temperature
        {
            get; set;
        }

        public float? Pressure
        {
            get; set;
        }

        public float? Humidity
        {
            get; set;
        }

        public string? SensorId
        {
            get; set;
        }

        #endregion
    }
}
