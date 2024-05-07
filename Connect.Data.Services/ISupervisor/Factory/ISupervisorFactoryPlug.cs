namespace Connect.Data
{
    public interface ISupervisorFactoryPlug
    {
        public ISupervisorPlug CreateSupervisor(int? cacheIsHandled);
    }
}
