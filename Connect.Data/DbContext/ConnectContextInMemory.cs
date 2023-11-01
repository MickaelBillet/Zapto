using Microsoft.EntityFrameworkCore;

namespace Connect.Data.DataContext
{
    public class ConnectContextInMemory : ConnectContext
	{
        public ConnectContextInMemory(DbContextOptions options) : base(options)
        { }
	}
}
