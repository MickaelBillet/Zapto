using Framework.Core.Domain;
using System;

namespace WeatherZapto.Model
{
    public class ZaptoWeather : Item
    {
        public string? Icon { get; set; }
        public string? WeatherText { get; set; }
        public string? Temperature { get; set; }
        public string? TemperatureMax { get; set; }
        public string? TemperatureMin { get; set; }
        public string? FeelsLike { get; set; }
        public string? WindSpeed { get; set; }
        public int? WindDirection { get; set; }
        public string? Location { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public DateTime? TimeSpam { get; set; }
        public int? Pressure { get; set; }
    }
}
