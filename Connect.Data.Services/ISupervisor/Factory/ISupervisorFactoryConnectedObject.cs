namespace Connect.Data
{
    public interface ISupervisorFactoryConnectedObject
    {
        public ISupervisorConnectedObject CreateSupervisor(int? cacheIsHandled);
    }
}
