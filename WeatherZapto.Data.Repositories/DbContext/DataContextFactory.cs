using Framework.Common.Services;
using Framework.Core.Base;
using Framework.Data.Abstractions;
using Framework.Infrastructure.Services;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using System.Collections.Concurrent;
using System.Data;

namespace WeatherZapto.Data.DataContext
{
    public class DataContextFactory : IDataContextFactory
    {
        private readonly ConcurrentBag<IDataContext?> _pool = new ConcurrentBag<IDataContext?>();
        private readonly int _maxPoolSize = 32;

        #region Properties
        protected ConnectionType ConnectionType { get; }
        #endregion

        #region Constructor
        public DataContextFactory(ISecretService secretService, string connectionStringKey, string serverTypeKey)
        {
            this.ConnectionType = ConnectionString.GetConnectionType(secretService, connectionStringKey, serverTypeKey);
        }
        public DataContextFactory(IServiceProvider provider, string connectionStringKey, string serverTypeKey)
        {
            ISecretService secretService = provider.GetRequiredService<ISecretService>();
            this.ConnectionType = ConnectionString.GetConnectionType(secretService, connectionStringKey, serverTypeKey);
        }
        public DataContextFactory(IServiceProvider provider)
        {
            IConfiguration configuration = provider.GetRequiredService<IConfiguration>();
            this.ConnectionType = ConnectionString.GetConnectionType(configuration);
        }
        #endregion

        #region Methods
        public IDataContext? GetContext()
        {
            if (_pool.TryTake(out var context))
            {
                return context;
            }
            return this.CreateDbContext();
        }
        public void ReturnContext(IDataContext? context)
        {
            if (_pool.Count < _maxPoolSize)
            {
                _pool.Add(context);
            }
            else
            {
                this.DisposeContext(context);
            }
        }
        private void DisposeContext(IDataContext? dataContext)
        {
            dataContext?.Dispose();
        }

        private IDataContext? CreateDbContext()
		{
			IDbConnection? connection = null;
			IDataContext? context = null;

			if (this.ConnectionType.ServerType == ServerType.PostgreSQL)
			{
				connection = new NpgsqlConnection(this.ConnectionType.ConnectionString);
				context = new WeatherZaptoContextPostGreSQL(connection);
            }
			else if (this.ConnectionType.ServerType == ServerType.SqlLite)
			{
				connection = new SqliteConnection(this.ConnectionType.ConnectionString);
				context = new WeatherZaptoContextSqlite(connection);
			}

			return context;
		}
        #endregion
    }
}
