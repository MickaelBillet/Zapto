using AirZapto.Data.DataContext;
using AirZapto.Data.Services.Repositories;
using Framework.Data.Abstractions;
using System;

namespace AirZapto.Data.Repositories
{
    public class RepositoryFactory : IRepositoryFactory
	{
		public Lazy<IRepository>? CreateRepository(IDataContext? context)
		{
			return ((context as AirZaptoContext) != null) ? new Lazy<IRepository>(() => new Repository(context as AirZaptoContext)) : null;
		}
	}
}
 