using Framework.Core.Base;
using System;

namespace Framework.Infrastructure.Services
{
    public interface IErrorHandlerService
    {
        public void DeleteError(string errorIdent, Action handleError);
        public void TriggerError(Error error, Action<Error> handleError);
        public void CleanError(Action clearError);
    }
}
