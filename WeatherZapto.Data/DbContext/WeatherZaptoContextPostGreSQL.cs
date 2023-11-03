using System.Data;
using Microsoft.EntityFrameworkCore;

namespace WeatherZapto.Data.DataContext
{
    public class WeatherZaptoContextPostGreSQL : WeatherZaptoContext
    {
		public WeatherZaptoContextPostGreSQL(IDbConnection connection) : base(connection)
		{
		}

        public WeatherZaptoContextPostGreSQL(DbContextOptions options) : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseNpgsql(this.Connection?.ConnectionString);
		}
	}
}
