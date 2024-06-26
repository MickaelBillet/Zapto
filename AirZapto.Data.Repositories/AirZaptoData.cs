﻿using AirZapto.Data.DataContext;
using AirZapto.Data.Repositories;
using AirZapto.Data.Services.Repositories;
using Framework.Data.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace AirZapto.Data
{
    public static class AirZaptoData
	{
		public static void AddRepositories(this IServiceCollection services)
		{
			services.AddTransient<IRepositoryFactory, RepositoryFactory>();
            services.AddTransient<IDataContextFactory, DataContextFactory>();
        }
    }
}

