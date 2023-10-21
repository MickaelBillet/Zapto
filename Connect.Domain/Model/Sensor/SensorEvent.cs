using System.Text.Json.Serialization;

namespace Connect.Model
{
    public class SensorEvent
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

        [JsonPropertyName("Leak")]
        public int Leak
        {
            get; set;
        }
    }
}
