namespace Connect.Data
{
    public interface ISupervisorFactoryConfiguration
    {
        public ISupervisorConfiguration CreateSupervisor(int? cacheIsHandled);
    }
}
