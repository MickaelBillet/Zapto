namespace Connect.Data
{
    public interface ISupervisorFactoryOperationRange
    {
        public ISupervisorOperationRange CreateSupervisor(int? cacheIsHandled);
    }
}
