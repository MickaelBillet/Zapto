using System.Text.Json.Serialization;

namespace Connect.Model
{
    public class PlugCommand
    {
        [JsonIgnore]
        public string? ProtocolType { get; set; }

        [JsonPropertyName("Adr")]
        public string? Address { get; set; }

        [JsonPropertyName("Unit")]
        public string? Unit { get; set; }

        [JsonIgnore]
        public string? Pin0 { get; set; }

        [JsonPropertyName("Prd")]
        public string? Period { get; set; }

        [JsonPropertyName("Odr")]
        public string? Order { get; set; }

	}
}
