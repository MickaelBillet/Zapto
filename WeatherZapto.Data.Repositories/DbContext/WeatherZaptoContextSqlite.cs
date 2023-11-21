using System.Data;
using Microsoft.EntityFrameworkCore;

namespace WeatherZapto.Data.DataContext
{
    public class WeatherZaptoContextSqlite : WeatherZaptoContext
    {
		public WeatherZaptoContextSqlite(IDbConnection connection) : base(connection)
		{
		}

        public WeatherZaptoContextSqlite(DbContextOptions options) : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlite(this.Connection?.ConnectionString);
		}
	}
}
