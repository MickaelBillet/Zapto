
using System.Text.Json.Serialization;

namespace AirZapto.Model
{
    public class SensorData
    {
        #region Property

        [JsonPropertyName("Temp")]
        public float Temperature
        {
            get; set;
        }

        [JsonPropertyName("Pres")]
        public float Pressure
        {
            get; set;
        }

        [JsonPropertyName("Humi")]
        public float Humidity
        {
            get; set;
        }

        [JsonPropertyName("CO2")]
        public int CO2
        {
            get; set;
        }

        #endregion
    }
}
