namespace Zapto.Component.Common.Models
{
    public record WeatherModel : BaseModel
    {
		public string? Image { get; set; }
		public string? WeatherText { get; set; }
        public string? Temperature { get; set; }
        public string? TemperatureMax { get; set; }
        public string? TemperatureMin { get; set; }
        public string? FeelsLike { get; set; }
        public string? WindSpeed { get; set; }
        public int? WindDirection { get; set; }
        public string? Location { get; set; }
        public int? Pressure { get; set; }
    }
}
