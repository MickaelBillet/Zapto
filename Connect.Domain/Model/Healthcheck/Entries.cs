using System.Text.Json.Serialization;

namespace Connect.Model.Healthcheck
{
    public class Entries
    {
        public Memory? Memory { get; set; }
        public Sqlite? Sqlite { get; set; }

        [JsonPropertyName("Server Iot Connection")]
        public ServerIotConnection? ServerIotConnection { get; set; }

        [JsonPropertyName("Error System")]
        public ErrorSystem? ErrorSystem { get; set; }
        public SignalR? SignalR { get; set; }

        [JsonPropertyName("Sensors Status")]
        public SensorsStatus? SensorsStatus { get; set; }
    }
}
