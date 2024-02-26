namespace WeatherZapto.Data
{
    public interface ISupervisorTemperature
    {
        Task<IEnumerable<double>> GetTemperatures(string location, DateTime day);
        Task<double?> GetTemperatureMin(string location, DateTime day);
        Task<double?> GetTemperatureMax(string location, DateTime day);
    }
}
