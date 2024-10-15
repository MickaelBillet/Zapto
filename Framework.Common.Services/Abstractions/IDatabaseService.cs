using System;
using System.Threading.Tasks;

namespace Framework.Infrastructure.Abstractions
{
    public interface IDatabaseService : IDisposable
	{
		Task ConfigureDatabase();
        public bool DatabaseIsInitialized();
        public bool DropDatabase();
    }
}
