using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Connect.Data.DataContext
{
    public class ConnectContextMySql : ConnectContext
	{
		public ConnectContextMySql(IDbConnection connection) : base(connection)
		{
			
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseMySql(this.Connection?.ConnectionString, new MySqlServerVersion(new Version(8, 0, 21)));
		}
	}
}
