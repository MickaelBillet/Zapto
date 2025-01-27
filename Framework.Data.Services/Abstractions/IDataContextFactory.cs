namespace Framework.Data.Abstractions
{
    public interface IDataContextFactory
    {
        public IDataContext? GetContext();
        public void ReturnContext(IDataContext? context);
    }
}
