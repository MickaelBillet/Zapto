using Framework.Common.Services;
using Framework.Core.Base;
using Microsoft.Extensions.Configuration;

namespace Framework.Infrastructure.Services
{
    public static class ConnectionString
    {
        public static (string connectionString, string serverName) GetConnectionString(IConfiguration configuration, ISecretService secretService)
        {
            string connectionString = string.Empty;
            string serverName = string.Empty;

            connectionString = secretService.GetSecret("ConnectionStrings");
            serverName = secretService.GetSecret("ServerType");

            return (connectionString, serverName);
        }

        public static ConnectionType GetConnectionType(IConfiguration configuration, ISecretService secretService)
        {
            ConnectionType connectionType;

            connectionType = new ConnectionType()
            {
                ConnectionString = secretService.GetSecret("ConnectionString"),
                ServerType = ConnectionType.GetServerType(secretService.GetSecret("ServerType"))
            };

            return connectionType;
        }
    }
}
