using System;
using System.Threading.Tasks;

namespace Framework.Data.Abstractions
{
    public static class DataContextFactoryExtension
    {
        public static async Task UseContext(this IDataContextFactory pool, Func<IDataContext?, Task> action) 
        {
            var context = pool.GetContext();
            try
            {
                await action(context);
            }
            finally
            {
                pool.ReturnContext(context);
            }
        }

        public static void UseContext(this IDataContextFactory pool, Action<IDataContext?> action)
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
