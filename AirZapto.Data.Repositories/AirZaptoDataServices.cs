using AirZapto.Data.DataContext;
using AirZapto.Data.Repositories;
using AirZapto.Data.Services.Repositories;
using Framework.Data.Abstractions;
using Framework.Data.Session;
using Microsoft.Extensions.DependencyInjection;

namespace AirZapto.Data
{
    public static class AirZaptoDataServices
	{
		public static void AddRepositories(this IServiceCollection services, string connectionStringKey, string serverTypeKey)
		{
			services.AddTransient<IRepositoryFactory, RepositoryFactory>();
            services.AddSingleton<IDataContextFactory, DataContextFactory>(provider => new DataContextFactory(provider, connectionStringKey, serverTypeKey));
            services.AddTransient<IDalSession, DalSession>(provider => new DalSession(provider, connectionStringKey, serverTypeKey));
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IRepositoryFactory, RepositoryFactory>();
            services.AddSingleton<IDataContextFactory, DataContextFactory>(provider => new DataContextFactory(provider));
            services.AddTransient<IDalSession, DalSession>(provider => new DalSession(provider));
        }
    }
}

