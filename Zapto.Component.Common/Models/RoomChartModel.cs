namespace Zapto.Component.Common.Models
{
    public class RoomChartModel : BaseModel
    {
        public IEnumerable<decimal?> TemperaturesIN { get; set; } = Enumerable.Empty<decimal?>();
        public IEnumerable<decimal?> TemperaturesOUT { get; set; } = Enumerable.Empty<decimal?>();
        public IEnumerable<decimal?> Humidities { get; set; } = Enumerable.Empty<decimal?>();
        public IEnumerable<string?> Labels { get; set; } = Enumerable.Empty<string?>();
        public string? Day { get; set; } = null;
    }
}
