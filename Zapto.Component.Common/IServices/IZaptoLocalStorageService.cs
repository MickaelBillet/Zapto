namespace Zapto.Component.Common.IServices
{
    public interface IZaptoLocalStorageService
    {
        Task<T?> GetItemAsync<T>(string key) where T : class;
        Task SetItemAsync<T>(string key, T value);
    }
}
