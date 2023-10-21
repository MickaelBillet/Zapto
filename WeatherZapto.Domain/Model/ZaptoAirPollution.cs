using Framework.Core.Domain;
using System;

namespace WeatherZapto.Model
{
    public class ZaptoAirPollution  : Item
    {
        public string? Location { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public double? aqi { get; set; }
        public double? co { get; set; }
        public double? no { get; set; }
        public double? no2 { get; set; }
        public double? o3 { get; set; }
        public double? so2 { get; set; }
        public double? pm2_5 { get; set; }
        public double? pm10 { get; set; }
        public double? nh3 { get; set; }
        public DateTime? TimeStamp { get; set; }
    }
}
