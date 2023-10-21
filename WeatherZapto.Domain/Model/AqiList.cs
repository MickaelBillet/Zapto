namespace WeatherZapto.Model
{
    public class AqiList
    {
        public int dt { get; set; }
        public Aqi? main { get; set; }
        public Components? components { get; set; }
    }
}
