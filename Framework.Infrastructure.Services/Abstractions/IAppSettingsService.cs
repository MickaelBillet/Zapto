using System.Collections.Specialized;

namespace Framework.Infrastructure.Services
{
    public interface IAppSettingsService
    {
        NameValueCollection ReadAllSettings();

        string ReadSetting(string key);
    }
}
