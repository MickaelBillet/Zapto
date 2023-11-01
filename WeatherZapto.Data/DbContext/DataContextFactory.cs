using Framework.Core.Base;
using Framework.Data.Abstractions;
using Microsoft.Data.Sqlite;
using Npgsql;
using System.Data;

namespace WeatherZapto.Data.DataContext
{
    public class DataContextFactory : IDataContextFactory
    {
		public (IDbConnection?, IDataContext?)? CreateDbContext(string? connectionString, ServerType server)
		{
			IDbConnection? connection = null;
			IDataContext? context = null;

			if (server == ServerType.PostgreSQL)
			{
				connection = new NpgsqlConnection(connectionString);
				context = new WeatherZaptoContextPostGreSQL(connection);

            }
			else if (server == ServerType.SqlLite)
			{
				connection = new SqliteConnection(connectionString);
				context = new WeatherZaptoContextSqlite(connection);
			}

			return (connection, context);
		}
	}
}
