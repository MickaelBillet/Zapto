using Framework.Core.Base;
using System;
using System.Threading.Tasks;

namespace Framework.Data.Abstractions
{
    public interface IDatabaseService : IDisposable
	{
        public bool IsInitialized { get; }
        bool DropDatabase(ConnectionType connectionType);
		bool CreateDatabase(ConnectionType connectionType);
		bool DatabaseExist(ConnectionType connectionType);
		Task FeedDataAsync();
		Task InitializeDataAsync();
		Task<bool> UpgradeDatabaseAsync();
		Task ConfigureDatabase();
    }
}
