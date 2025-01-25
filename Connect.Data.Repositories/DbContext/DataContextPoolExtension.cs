using Framework.Data.Abstractions;
using System.Data;

namespace Connect.Data.DataContext
{
    public static class DataContextPoolExtension
    {
        public static void UseContext(this IDataContextFactory pool, Action<(IDbConnection? connection, IDataContext? datacontext)?> action) 
        {
            var context = pool.GetContext();
            try
            {
                action(context);
            }
            finally
            {
                pool.ReturnContext(context);
            }
        }
    }
}
