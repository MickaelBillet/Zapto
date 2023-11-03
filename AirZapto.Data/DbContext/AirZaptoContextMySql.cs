using Microsoft.EntityFrameworkCore;
using System;
using System.Data;

namespace AirZapto.Data.DataContext
{
    public class AirZaptoContextMySql : AirZaptoContext
    {
        public AirZaptoContextMySql(IDbConnection connection) : base(connection)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(this.Connection?.ConnectionString, new MySqlServerVersion(new Version(8, 0, 21)));
        }
    }
}
