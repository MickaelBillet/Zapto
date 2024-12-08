using AirZapto.Data.Services;

namespace AirZapto.Data
{
    public interface ISupervisorFactoryVersion
    {
        ISupervisorVersion CreateSupervisor();
    }
}