using AirZapto.Data.DataContext;
using AirZapto.Data.Services.Repositories;
using Framework.Data.Abstractions;
using System;
using System.Data;

namespace AirZapto.Data.Repositories
{
    public class RepositoryFactory : IRepositoryFactory
	{
		public Lazy<IRepository>? CreateRepository(IDalSession session, IDataContextFactory contextFactory)
		{
            (IDbConnection? connection, IDataContext? context)? obj = (session != null) ? contextFactory.CreateDbContext(session.ConnectionType?.ConnectionString, session.ConnectionType?.ServerType) : null;
            return ((obj != null) &&(obj.Value.context as AirZaptoContext) != null) ? new Lazy<IRepository>(() => new Repository(obj.Value.context as AirZaptoContext)) : null;
        }
    }
}
 