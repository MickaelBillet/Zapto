using System.Data;
using Microsoft.EntityFrameworkCore;

namespace AirZapto.Data.DataContext
{
    public class AirZaptoContextSqlite : AirZaptoContext
    {
        public AirZaptoContextSqlite(IDbConnection connection) : base(connection)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(this.Connection?.ConnectionString);
        }
    }
}
