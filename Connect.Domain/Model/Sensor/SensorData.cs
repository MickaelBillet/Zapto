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

        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
        [JsonPropertyName("Temp")]
        public float Temperature
        {
            get; set;
        }

        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
        [JsonPropertyName("Pres")]
        public float Pressure
        {
            get; set;
        }

        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
        [JsonPropertyName("Humi")]
        public float Humidity
        {
            get; set;
        }
    }
}
