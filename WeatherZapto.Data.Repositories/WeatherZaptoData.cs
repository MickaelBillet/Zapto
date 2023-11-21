using Connect.Data.Repositories;
using Framework.Data.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using WeatherZapto.Data.DataContext;
using WeatherZapto.Data.Services.Repositories;

namespace WeatherZapto.Data.Repository
{
    public static class WeatherZaptoData
	{
		public static void AddRepositories(this IServiceCollection services)
		{
            services.AddTransient<IRepositoryFactory, RepositoryFactory>();
            services.AddTransient<IDataContextFactory, DataContextFactory>();
        }
    }
}

