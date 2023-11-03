using Framework.Core.Base;
using Framework.Data.Abstractions;
using Microsoft.Data.Sqlite;
using MySqlConnector;
using System.Data;

namespace Connect.Data.DataContext
{
    public class DataContextFactory : IDataContextFactory
    {
		public (IDbConnection?, IDataContext?)? CreateDbContext(string? connectionString, ServerType server)
		{
			IDbConnection? connection = null;
			IDataContext? context = null;

			if (server == ServerType.MySql)
			{
				connection = new MySqlConnection(connectionString);
				context = new ConnectContextMySql(connection);
			}
			else if (server == ServerType.SqlLite)
			{
				connection = new SqliteConnection(connectionString);
				context = new ConnectContextSqlite(connection);
			}

			return (connection, context);
		}
	}
}
