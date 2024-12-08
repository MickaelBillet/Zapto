using AirZapto.Data.Services;

namespace AirZapto.Data
{
    public interface ISupervisorFactorySensorData
    {
        ISupervisorSensorData CreateSupervisor();
    }
}