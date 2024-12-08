using System.Text.Json.Serialization;

namespace Connect.Model
{
    public struct SensorStatus
    {
        #region Property
        public static string Name = "SensorStatus";

        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
        public double? Temperature
        {
            get; set;
        }

        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
        public double? Pressure
        {
            get; set;
        }

        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
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
