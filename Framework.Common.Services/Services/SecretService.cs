using Framework.Common.Services;
using Microsoft.Extensions.Configuration;

namespace Framework.Infrastructure.Services
{
    public static class SecretService
    {
        public static ISecretService? GetSecretService(IConfiguration configuration)
        {
            ISecretService? service = null;
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
