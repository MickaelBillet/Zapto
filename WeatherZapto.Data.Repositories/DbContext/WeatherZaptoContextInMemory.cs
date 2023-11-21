using Microsoft.EntityFrameworkCore;
using WeatherZapto.Data.DataContext;

namespace Connect.Data.DataContext
{
    public class WeatherZaptoContextInMemory : WeatherZaptoContext
	{
        public WeatherZaptoContextInMemory(DbContextOptions options) : base(options)
        { }
	}
}
