namespace Connect.Model
{
    public struct SensorStatus
    {
        #region Property
        public static string Name = "SensorStatus";

        public double? Temperature
        {
            get; set;
        }

        public double? Pressure
        {
            get; set;
        }

        public double? Humidity
        {
            get; set;
        }

        public string SensorId
        {
            get; set;
        }

        public string RoomId
        {
            get; set;
        }

        public byte? IsRunning { get; set; }

        public byte? LeakDetected { get; set; }
        #endregion
    }
}
