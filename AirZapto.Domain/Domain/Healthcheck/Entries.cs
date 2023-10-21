using System.Text.Json.Serialization;

namespace AirZapto.Model.Healthcheck
{
    public class Entries
    {
        public Memory? Memory { get; set; }
        public Sqlite? Sqlite { get; set; }

        [JsonPropertyName("Error System")]
        public ErrorSystem? ErrorSystem { get; set; }

        [JsonPropertyName("Sensors Status")]
        public SensorsStatus? SensorsStatus { get; set; }
    }
}
