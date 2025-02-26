using Framework.Common.Services;
using Framework.Core.Base;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Infrastructure.Services
{
    public static class FrameworkCommonServices
	{
        public static void AddMailService(this IServiceCollection services)
        {
            services.AddTransient<IMailService, MailService>();
        }

        public static void AddAppSettingsService(this IServiceCollection services)
		{
			services.AddSingleton<IAppSettingsService, AppSettingsService>();
		}

		public static void AddJsonFileService(this IServiceCollection services)
		{
			services.AddTransient<IJsonFileService, JsonFileService>();
		}

		public static void AddUdpCommunicationService(this IServiceCollection services)
		{
			services.AddTransient<IUdpCommunicationService, UdpCommunicationService>();
		}

        public static void AddThreadSynchronizationService(this IServiceCollection services)
		{
			services.AddTransient<IThreadSynchronizationService, ThreadSynchronizationService>();
		}

		public static void AddSecretService(this IServiceCollection services, IConfiguration configuration, string secret)
		{
			if (secret == SecretType.VarEnv)
			{
				services.AddTransient<ISecretService, VarEnvService>();
			}
			else if (secret == SecretType.KeyVault)
			{
				services.AddTransient<ISecretService, KeyVaultService>((provider) => new KeyVaultService(configuration));
			}
		}

		public static void AddCommunicationService(this IServiceCollection services, string communicationType, string portName, int baudRate)
        {
            if (communicationType == CommunicationType.WebSocket)
            {
                services.AddScoped<IWSMessageManager, WebSocketMessageManager>();
                services.AddSingleton<IWebSocketService, WebSocketService>();
            }
            else if (communicationType == CommunicationType.Serial)
            {
                services.AddScoped<ISerialCommunicationService, SerialCommunicationService>((provider) => new SerialCommunicationService(provider, portName, baudRate));
            }
        }
    }
}
