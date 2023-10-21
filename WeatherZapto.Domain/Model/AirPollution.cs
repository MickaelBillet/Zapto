using System.Collections.Generic;

namespace WeatherZapto.Model
{
    public class AirPollution
    {
        public Coord? coord { get; set; }
        public List<AqiList>? list { get; set; }
    }
}
