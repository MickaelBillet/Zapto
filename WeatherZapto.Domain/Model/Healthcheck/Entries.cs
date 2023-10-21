using System.Text.Json.Serialization;

namespace WeatherZapto.Model.Healthcheck
{
    public class Entries
    {
        public Memory? Memory { get; set; }
        public PostGreSql? PostGreSql { get; set; }
        [JsonPropertyName("Error System")]
        public ErrorSystem? ErrorSystem { get; set; }
    }
}
