using Framework.Common.Services;
using Framework.Core.Base;
using Microsoft.Extensions.Configuration;

namespace Framework.Infrastructure.Services
{
    public static class ConnectionString
    {
        public static (string connectionString, string serverName) GetConnectionString(IConfiguration configuration, 
                                                                                        string connectionStringKey, 
                                                                                        string serverTypeKey)
        {
            string connectionString = string.Empty;
            string serverName = string.Empty;
            ISecretService secretService = GetSecretService(configuration);
            connectionString = secretService.GetSecret(connectionStringKey);
            serverName = secretService.GetSecret(serverTypeKey);
            return (connectionString, serverName);
        }

        public static ConnectionType GetConnectionType(IConfiguration configuration, 
                                                        ISecretService secretService, 
                                                        string connectionStringKey, 
                                                        string serverTypeKey)
        {
            ConnectionType connectionType = new ConnectionType()
            {
                ConnectionString = secretService.GetSecret(connectionStringKey),
                ServerType = ConnectionType.GetServerType(secretService.GetSecret(serverTypeKey))
            };
            return connectionType;
        }
        public static ConnectionType GetConnectionType(IConfiguration configuration)
        {
            ConnectionType connectionType = new ConnectionType()
            {
                ConnectionString = configuration["ConnectionStrings:DefaultConnection"],
                ServerType = ConnectionType.GetServerType(configuration["ConnectionStrings:ServerType"])
            };
            return connectionType;
        }
        private static ISecretService GetSecretService(IConfiguration configuration)
        {
            ISecretService service = null;
            if (byte.Parse(configuration["Secret"]) == 1)
            {
                service = new VarEnvService();
            }
            else if (byte.Parse(configuration["Secret"]) == 2)
            {
                service = new KeyVaultService(configuration);
            }
            return service;
        }
    }
}
