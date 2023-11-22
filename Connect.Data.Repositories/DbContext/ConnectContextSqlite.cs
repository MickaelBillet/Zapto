using System.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Connect.Data.DataContext
{
    public class ConnectContextSqlite : ConnectContext
	{
        public ConnectContextSqlite(IDbConnection connection) : base(connection) { }
        public ConnectContextSqlite(DbContextOptions options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlite(this.Connection?.ConnectionString);
		}
        public override async Task<int> ExecuteNonQueryAsync(string sql)
        {
            int res = -1;
            if (this.Connection != null)
            {
                using (SqliteConnection? connection = this.Connection as SqliteConnection)
                {
                    if (connection != null)
                    {
                        await connection.OpenAsync();

                        if (connection.State == ConnectionState.Open)
                        {
                            using (SqliteCommand sqlQuery = new SqliteCommand(sql, connection))
                            {
                                res = await sqlQuery.ExecuteNonQueryAsync();
                            }
                        }
                    }
                }
            }
            return res;
        }
    }
}
