namespace Connect.Data
{
    public interface ISupervisorFactoryRoom
    {
        public ISupervisorRoom CreateSupervisor(byte? cacheIsHandled);
    }
}
