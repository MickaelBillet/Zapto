using System.Collections.Generic;

namespace Connect.Model.Healthcheck
{
    public class Sqlite
    {
        public Data? Data { get; set; }
        public object? Description { get; set; }
        public string? Duration { get; set; }
        public object? Exception { get; set; }
        public int? Status { get; set; }
        public List<string>? Tags { get; set; }
    }
}
