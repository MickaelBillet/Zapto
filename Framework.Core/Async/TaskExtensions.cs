using System;
using System.Threading.Tasks;

namespace Framework.Core.Base
{
    public static class TaskExtensions
    {
        public static async void FireAndForgetSafeAsync(this Task task, IErrorHandler handler = null)
        {
            try
            {
                await task.ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                handler?.HandleError(ex);
            }
        }
    }

    public interface IErrorHandler
    {
        void HandleError(Exception ex);
    }
}
