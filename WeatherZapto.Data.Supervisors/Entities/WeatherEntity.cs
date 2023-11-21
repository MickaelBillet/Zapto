using Framework.Core.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeatherZapto.Data.Entities
{
    [Table("Weather")]
    public class WeatherEntity : ItemEntity
    {
        public string Icon { get; set; }
        public string WeatherText { get; set; }
        public string Temperature { get; set; }
        public string WindSpeed { get; set; }
        public int? WindDirection { get; set; }
        public string Location { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public DateTime? TimeStamp { get; set; }
    }
}
