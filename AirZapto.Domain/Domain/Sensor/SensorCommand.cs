
using System.Text.Json.Serialization;

namespace AirZapto.Model
{
    public class SensorCommand
    {
        #region Property

        [JsonPropertyName("Cmd")]
        public int Command
        {
            get; set;
        }

        [JsonPropertyName("Chan")]
        public string? Channel
        {
            get; set;
        }

        [JsonPropertyName("Name")]
        public string? Name
        {
            get; set;
        }

        [JsonPropertyName("Prd")]
        public string? Period
        {
            get; set;
        }

        #endregion
    }
}
