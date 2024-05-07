using Framework.Common.Services;
using Framework.Core.Base;
using Microsoft.Extensions.Configuration;

namespace Framework.Infrastructure.Services
{
    public static class ConnectionString
    {
        public static (string connectionString, string serverName) GetConnectionString(IConfiguration configuration, IKeyVaultService keyVaultService)
        {
            string connectionString = string.Empty;
            string serverName = string.Empty;

            if ((string.IsNullOrEmpty(configuration["KeyVault"]) == false) && (int.Parse(configuration["KeyVault"]!) == 1))
            {
                connectionString = configuration["ConnectionStrings:DefaultConnection"]!;
                serverName = configuration["ConnectionStrings.ServerType"]!;
            }
            else
            {
                connectionString = keyVaultService.GetSecret("ConnectionStrings");
                serverName = keyVaultService.GetSecret("ServerType");
            }

            return (connectionString, serverName);
        }

        public static ConnectionType GetConnectionType(IConfiguration configuration, IKeyVaultService keyVaultService)
        {
            ConnectionType connectionType;

            if ((string.IsNullOrEmpty(configuration["KeyVault"]) == false) && (int.Parse(configuration["KeyVault"]!) == 1))
            {
                connectionType = new ConnectionType()
                {
                    ConnectionString = keyVaultService.GetSecret("ConnectionString"),
                    ServerType = ConnectionType.GetServerType(keyVaultService.GetSecret("ServerType"))
                };
            }
            else
            {
                connectionType = new ConnectionType()
                {
                    ConnectionString = configuration["ConnectionStrings:DefaultConnection"],
                    ServerType = ConnectionType.GetServerType(configuration["ConnectionStrings:ServerType"])
                };
            }

            return connectionType;
        }
    }
}
