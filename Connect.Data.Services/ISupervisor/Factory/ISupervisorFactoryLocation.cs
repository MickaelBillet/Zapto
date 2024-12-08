namespace Connect.Data
{
    public interface ISupervisorFactoryLocation
    {
        public ISupervisorLocation CreateSupervisor(int? cacheIsHandled);
    }
}
