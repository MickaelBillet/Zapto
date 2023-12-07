using System.Collections.Generic;

namespace WeatherZapto.Model.Healthcheck
{
    public class OpenWeatherCalls
    {
        public Data? Data { get; set; }
        public string? Description { get; set; }
        public string? Duration { get; set; }
        public object? Exception { get; set; }
        public int? Status { get; set; }
        public List<string>? Tags { get; set; }
    }
}
