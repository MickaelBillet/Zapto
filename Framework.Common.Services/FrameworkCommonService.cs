using Microsoft.Extensions.DependencyInjection;

namespace Framework.Infrastructure.Services
{
    public static class FrameworkCommonService
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

		public static void AddUdpService(this IServiceCollection services)
		{
			services.AddTransient<IUdpCommunicationService, UdpCommunicationService>();
		}

		public static void AddThreadSynchronizationService(this IServiceCollection services)
		{
			services.AddTransient<IThreadSynchronizationService, ThreadSynchronizationService>();
		}
	}
}
