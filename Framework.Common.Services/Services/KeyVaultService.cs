using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Framework.Common.Services;
using Microsoft.Extensions.Configuration;
using System;
#nullable disable

namespace Framework.Infrastructure.Services
{
    public class KeyVaultService : ISecretService
    {
        #region Properties
        private string ClientId { get; }
        private string TenantId { get; }
        private string KeyVault_ID { get; }
        private string AppRegistration { get; }
        #endregion

        #region Constructor
        public KeyVaultService(IConfiguration configuration)
        {
            this.ClientId = configuration["Client_ID"];
            this.TenantId = configuration["Tenant_ID"];
            this.KeyVault_ID = configuration["KeyVault_ID"];
            this.AppRegistration = configuration["AppRegistration"];

        }
        #endregion

        #region Methods
        public string GetSecret(string name)
        {
            string result = null;
            ClientSecretCredential credential = new ClientSecretCredential(this.TenantId, this.ClientId, this.KeyVault_ID);
            if (credential != null)
            {
                SecretClient client = new SecretClient(new Uri($"https://{this.AppRegistration}.vault.azure.net"), credential);
                if (client != null)
                {
                    KeyVaultSecret secret = client.GetSecret(name);
                    result = (secret != null) ? secret.Value : null;
                }
            }
            return result;
        }

        #endregion
    }
}
