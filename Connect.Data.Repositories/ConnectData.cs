using Connect.Data.DataContext;
using Connect.Data.Repositories;
using Connect.Data.Services.Repositories;
using Framework.Data.Abstractions;
using Framework.Data.Session;
using Microsoft.Extensions.DependencyInjection;

namespace Connect.Data.Repository
{
    public static class ConnectData
	{
		public static void AddRepositories(this IServiceCollection services, string connectionStringKey, string serverTypeKey)
		{
            services.AddTransient<IRepositoryFactory, RepositoryFactory>();
            services.AddTransient<IDataContextFactory, DataContextFactory>();
            services.AddTransient<IDalSession, DalSession>(provider => new DalSession(provider, connectionStringKey, serverTypeKey));
        }
    }
}

