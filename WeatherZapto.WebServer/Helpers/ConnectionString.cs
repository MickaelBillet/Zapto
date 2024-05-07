using Framework.Common.Services;
using Framework.Infrastructure.Services;

namespace WeatherZapto.WebServer.Helpers
{
    public static class ConnectionString
    {
        public static (string connectionString, string serverName) GetConnectionString(IConfiguration configuration, IKeyVaultService keyVaultService)
        {
            string connectionString = string.Empty;
            string serverName = string.Empty;

#if DEBUG
            connectionString = configuration["ConnectionStrings:DefaultConnection"]!;
            serverName = configuration["ConnectionStrings.ServerType"]!;
#else
connectionString = new KeyVaultService(configuration).GetSecret("ConnectionStrings");
serverName = new KeyVaultService(configuration).GetSecret("ServerType");
#endif

            return (connectionString, serverName);
        }
    }
}
