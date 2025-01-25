using System;
using System.Data;

namespace Framework.Data.Abstractions
{
    public interface IDataContextFactory
    {
        public void UseContext(Action<(IDbConnection? connection, IDataContext? dataContext)?> action);
    }
}
