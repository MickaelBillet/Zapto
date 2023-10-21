namespace Connect.Model
{
    public struct RoomStatus
    {
        #region Property

        public static string Name = "RoomStatus";

        public double? Humidity
        {
            get; set;
        }

        public double? Temperature
        {
            get; set;
        }

        public double? Pressure
        {
            get; set;
        }

        public string RoomId
        {
            get; set;
        }

        #endregion
    }
}
