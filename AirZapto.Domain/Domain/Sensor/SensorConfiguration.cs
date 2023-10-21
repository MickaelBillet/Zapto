
using System.Text.Json.Serialization;

namespace AirZapto.Model
{
    public class SensorConfiguration
    {
        #region Property

        [JsonPropertyName("Name")]
        public string? Name
        {
            get; set;
        }

        [JsonPropertyName("Des")]
        public string? Description
        {
            get; set;
        }

        [JsonPropertyName("Chan")]
        public int Channel
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
