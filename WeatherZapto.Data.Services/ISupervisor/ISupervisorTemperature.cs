namespace WeatherZapto.Data
{
    public interface ISupervisorTemperature
    {
        Task<IEnumerable<float>> GetTemperatures(string location, DateTime day);
        Task<float?> GetTemperatureMin(string location, DateTime day);
        Task<float?> GetTemperatureMax(string location, DateTime day);
    }
}
