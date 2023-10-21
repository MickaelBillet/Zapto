using System.Text.Json.Serialization;

namespace Connect.Model
{
    public class SensorData
    {
        public string? Type
        {
            get; set;
        }

        [JsonPropertyName("Chan")]
        public string? Channel
        {
            get; set;
        }

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
    }
}
