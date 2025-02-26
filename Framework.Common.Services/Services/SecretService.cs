using Framework.Common.Services;
using Framework.Core.Base;
using Microsoft.Extensions.Configuration;

namespace Framework.Infrastructure.Services
{
    public static class SecretService
    {
        public static ISecretService? GetSecretService(IConfiguration configuration)
        {
            ISecretService? service = null;
            if (configuration["Secret"] == SecretType.VarEnv)
            {
                service = new VarEnvService();
            }
            else if (configuration["Secret"] == SecretType.KeyVault)
            {
                service = new KeyVaultService(configuration);
            }
            return service;
        }
    }
}
