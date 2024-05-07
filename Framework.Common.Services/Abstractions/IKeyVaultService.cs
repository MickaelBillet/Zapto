namespace Framework.Common.Services
{
    public interface IKeyVaultService
    {
        string GetSecret(string name);
    }
}
