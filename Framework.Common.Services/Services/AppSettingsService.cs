using Serilog;
using System;
using System.Collections.Specialized;
using System.Configuration;

#nullable disable

namespace Framework.Infrastructure.Services
{
    internal class AppSettingsService : IAppSettingsService
    {
        #region Properties

        #endregion

        #region Constructor

        public AppSettingsService(IServiceProvider serviceProvider)
        {
        }

        #endregion

        #region Methods

        public NameValueCollection ReadAllSettings()
        {
            NameValueCollection appSettings = null;

            try
            {
                appSettings = ConfigurationManager.AppSettings;

                if (appSettings.Count == 0)
                {
                    Log.Information("AppSettings is empty.");
                }
                else
                {
                    foreach (string key in appSettings.AllKeys)
                    {
                        Log.Information("Key: {0} Value: {1}", key, appSettings[key]);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "AppSettingsService.ReadAllSettings");
            }

            return appSettings;
        }

        public string ReadSetting(string key)
        {
            string result = null;
            try
            {
                NameValueCollection appSettings = ConfigurationManager.AppSettings;
                result = appSettings[key] ?? "Not Found";
                Log.Information(result);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "AppSettingsService.ReadSetting");
            }

            return result;
        }

        #endregion
    }
}
