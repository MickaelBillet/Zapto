using Framework.Core.Base;
using Framework.Infrastructure.Services;
using System;
using System.Collections.Concurrent;

namespace AirZapto.Services
{
    internal class ErrorHandlerService : IErrorHandlerService
    {
        #region Properties

        private ConcurrentDictionary<string, Error> ErrorsDictionnary
        {
            get; set;
        }

		#endregion

		#region Constructor

		public ErrorHandlerService()
        {
            this.ErrorsDictionnary = new ConcurrentDictionary<string, Error>();
        }

        #endregion

        #region Methods   
        
        public void CleanError(Action clearError)
		{
            this.ErrorsDictionnary.Clear();

            if (clearError != null)
            {
                clearError();
            }
        }

        public void DeleteError(string errorIdent, Action handleError)
        {
            if (errorIdent != null)
            {
                if (this.ErrorsDictionnary.TryRemove(errorIdent, out Error error))
                {
                    if (handleError != null)
                    {
                        handleError();
                    }
                }
            }
        }

        public void TriggerError(Error error, Action<Error> handleError)
        {
            if (error != null)
            {
                if ((error.IsRecorded) && (error.Ident != null))
                {
                    this.ErrorsDictionnary.TryAdd(error.Ident, error);
                }

                if (error.IsNotified)
                {
                    handleError(error);
                }
            }
        }

        #endregion
    }
 }
