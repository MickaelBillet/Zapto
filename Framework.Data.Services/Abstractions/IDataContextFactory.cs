using Framework.Core.Base;
using System.Data;

namespace Framework.Data.Abstractions
{
    public interface IDataContextFactory
    {
        public (IDbConnection? connection, IDataContext? context)? CreateDbContext(string? connectionString, ServerType? server);
    }
}
