using System;
using System.Threading.Tasks;

namespace Framework.Data.Abstractions
{
    public static class IDataContextFactorylExtension
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
    }
}
