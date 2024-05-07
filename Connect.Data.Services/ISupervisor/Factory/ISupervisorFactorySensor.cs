namespace Connect.Data
{
    public interface ISupervisorFactorySensor
    {
        public ISupervisorSensor CreateSupervisor(int? cacheIsHandled);
    }
}
