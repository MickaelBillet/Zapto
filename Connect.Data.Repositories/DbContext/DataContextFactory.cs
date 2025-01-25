using Framework.Common.Services;
using Framework.Core.Base;
using Framework.Data.Abstractions;
using Framework.Infrastructure.Services;
using Microsoft.Data.Sqlite;
using MySqlConnector;
using System.Collections.Concurrent;
using System.Data;

namespace Connect.Data.DataContext
{
    public class DataContextFactory
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
        #endregion

        #region Methods
        private IDataContext? CreateDbContext()
		{
			IDbConnection? connection = null;
			IDataContext? context = null;

            if (this.ConnectionType.ServerType == ServerType.MySql)
            {
                connection = new MySqlConnection(this.ConnectionType.ConnectionString);
                context = new ConnectContextMySql(connection);
            }
            else if (this.ConnectionType.ServerType == ServerType.SqlLite)
            {
                connection = new SqliteConnection(this.ConnectionType.ConnectionString);
                context = new ConnectContextSqlite(connection);
            }

			return context;
		}

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
                DisposeContext(context);
            }
        }

        private void DisposeContext(IDataContext? dataContext)
        {
            dataContext?.Dispose();
        }
        #endregion
    }
}
