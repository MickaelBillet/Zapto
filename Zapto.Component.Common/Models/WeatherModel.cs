namespace Zapto.Component.Common.Models
{
    public class WeatherModel : BaseModel
    {
		public string? Image { get; set; }
		public string? WeatherText { get; set; }
        public string? Temperature { get; set; }
        public string? WindSpeed { get; set; }
        public int? WindDirection { get; set; }
        public string? Location { get; set; }
    }
}
