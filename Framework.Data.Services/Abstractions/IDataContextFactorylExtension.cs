using System;

namespace Framework.Data.Abstractions
{
    public static class IDataContextFactorylExtension
    {
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
