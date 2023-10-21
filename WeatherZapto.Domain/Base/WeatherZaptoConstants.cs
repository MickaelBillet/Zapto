namespace WeatherZapto
{
    public static class WeatherZaptoConstants
    {
        public const string UrlOWWeatherCurrent = @"data/2.5/weather?lat={0}&lon={1}&appid={2}";
        public const string UrlOWWeatherCurrentLang = @"data/2.5/weather?lat={0}&lon={1}&appid={2}&lang={3}";
        public const string UrlOWWeatherImage = @"https://openweathermap.org/img/wn/{0}@2x.png";
        public const string UrlOWAirPollutionCurrent = @"data/2.5/air_pollution?lat={0}&lon={1}&appid={2}";
        public const string UrlOWLocation = @"geo/1.0/reverse?lat={0}&lon={1}&limit={2}&appid={3}";

        public const string RestUrlAirPollution = @"airpollution/lon={0}&lat={1}";
        public const string RestUrlAirPollutionLocation = @"airpollution/location={0}&lon={1}&lat={2}";
        public const string RestUrlLocation = @"location/lon={0}&lat={1}";
        public const string RestUrlWeather = @"weather/lon={0}&lat={1}&cul={2}";
        public const string RestUrlWeatherLocation = @"weather/location={0}&lon={1}&lat={2}&cul={3}";
        public const string RestUrlHealthCheck = @"health";

        public const string Application_Prefix = @"/WeatherZapto";
    }
}
