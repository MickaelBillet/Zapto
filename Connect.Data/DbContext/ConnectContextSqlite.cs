using System.Data;
using Microsoft.EntityFrameworkCore;

namespace Connect.Data.DataContext
{
    public class ConnectContextSqlite : ConnectContext
	{
		public ConnectContextSqlite(IDbConnection connection) : base(connection)
		{
		}

        public ConnectContextSqlite(DbContextOptions options) : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlite(this.Connection?.ConnectionString);
		}
	}
}
