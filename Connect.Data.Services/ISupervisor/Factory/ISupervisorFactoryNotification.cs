namespace Connect.Data
{
    public interface ISupervisorFactoryNotification
    {
        public ISupervisorNotification CreateSupervisor(int? cacheIsHandled);
    }
}
