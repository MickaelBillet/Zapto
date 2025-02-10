using System;
using System.Threading.Tasks;

namespace Framework.Infrastructure.Abstractions
{
    public interface IDatabaseService : IDisposable
	{
		Task ConfigureDatabase(int major, int minor, int build);
        public bool DatabaseIsInitialized();
        public bool DropDatabase();
    }
}
