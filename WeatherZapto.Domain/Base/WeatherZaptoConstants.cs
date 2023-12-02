namespace WeatherZapto
{
    public static class WeatherZaptoConstants
    {
        public const string UrlOWWeatherCurrent = @"data/2.5/weather?lat={0}&lon={1}&appid={2}";
        public const string UrlOWWeatherCurrentLang = @"data/2.5/weather?lat={0}&lon={1}&appid={2}&lang={3}";
        public const string UrlOWWeatherImage = @"https://openweathermap.org/img/wn/{0}@2x.png";
        public const string UrlOWAirPollutionCurrent = @"data/2.5/air_pollution?lat={0}&lon={1}&appid={2}";
        public const string UrlOWReverseLocation = @"geo/1.0/reverse?lat={0}&lon={1}&limit={2}&appid={3}";
        public const string UrlOWLocationZipCode = @"geo/1.0/zip?zip={0},{1}&appid={2}";
        public const string UrlOWLocationCity = @"geo/1.0/direct?q={0},{1},{2}&limit={3}&appid={4}";

        public const string UrlAirPollution = @"airpollution/lon={0}&lat={1}";
        public const string UrlAirPollutionLocation = @"airpollution/location={0}&lon={1}&lat={2}";
        public const string UrlReverseLocation = @"location/lon={0}&lat={1}";
        public const string UrlLocationZipCode1 = @"location/zip={0}";
        public const string UrlLocationZipCode2 = @"location/zip={0}&country={1}";
        public const string UrlLocationCity2 = @"location/city={0}&country={1}";
        public const string UrlLocationCity1 = @"location/city={0}";
        public const string UrlWeather = @"weather/lon={0}&lat={1}&cul={2}";
        public const string UrlWeatherLocation = @"weather/location={0}&lon={1}&lat={2}&cul={3}";
        public const string UrlHealthCheck = @"health";

        public const string Application_Prefix = @"/WeatherZapto";
    }
}
