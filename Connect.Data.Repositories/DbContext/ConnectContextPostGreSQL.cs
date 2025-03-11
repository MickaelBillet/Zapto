using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Data;

namespace Connect.Data.DataContext
{
    public class ConnectContextPostGreSQL : ConnectContext
    {
        public ConnectContextPostGreSQL(IDbConnection connection) : base(connection) { }
        public ConnectContextPostGreSQL(DbContextOptions options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(this.Connection?.ConnectionString);
        }
        public override async Task<int> ExecuteNonQueryAsync(string sql)
        {
            int res = -1;
            if (this.Connection != null)
            {
                using (NpgsqlConnection? connection = this.Connection as NpgsqlConnection)
                {
                    if (connection != null)
                    {
                        await connection.OpenAsync();

                        if (connection.State == ConnectionState.Open)
                        {
                            using (NpgsqlCommand sqlQuery = new NpgsqlCommand(sql, connection))
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
