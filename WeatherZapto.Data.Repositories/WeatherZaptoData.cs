using Connect.Data.Repositories;
using Framework.Data.Abstractions;
using Framework.Data.Session;
using Microsoft.Extensions.DependencyInjection;
using WeatherZapto.Data.DataContext;
using WeatherZapto.Data.Services.Repositories;

namespace WeatherZapto.Data.Repository
{
    public static class WeatherZaptoData
	{
		public static void AddRepositories(this IServiceCollection services, string connectionStringKey, string serverTypeKey)
		{
            services.AddTransient<IRepositoryFactory, RepositoryFactory>();
            services.AddTransient<IDataContextFactory, DataContextFactory>(provider => new DataContextFactory(provider, connectionStringKey, serverTypeKey));
            services.AddTransient<IDalSession, DalSession>(provider => new DalSession(provider, connectionStringKey, serverTypeKey));
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IRepositoryFactory, RepositoryFactory>();
            services.AddTransient<IDataContextFactory, DataContextFactory>(provider => new DataContextFactory(provider));
            services.AddTransient<IDalSession, DalSession>(provider => new DalSession(provider));
        }
    }
}

