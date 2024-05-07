namespace Connect.Data
{
    public interface ISupervisorFactoryProgram
    {
        public ISupervisorProgram CreateSupervisor(int? cacheIsHandled);
    }
}
