namespace Connect.Data.Supervisors
{
    public abstract class Supervisor
    {
        protected Supervisor() { }

        public async Task Initialize()
        {
            await Task.CompletedTask;
        }
    }
}
