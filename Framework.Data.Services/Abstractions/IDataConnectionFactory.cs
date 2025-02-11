using System.Data;

namespace Framework.Data.Abstractions
{
    public interface IDataConnectionFactory
    {
        public IDbConnection? CreateDbConnection();
    }
}