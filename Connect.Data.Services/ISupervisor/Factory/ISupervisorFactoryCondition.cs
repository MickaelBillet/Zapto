namespace Connect.Data
{
    public interface ISupervisorFactoryCondition
    {
        public ISupervisorCondition CreateSupervisor(int? cacheIsHandled);
    }
}
