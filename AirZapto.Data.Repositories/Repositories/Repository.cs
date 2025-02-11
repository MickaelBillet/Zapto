using Framework.Data.Abstractions;

namespace AirZapto.Data.Repositories
{
    public abstract class Repository
	{
        #region Properties
        protected IDataContextFactory DataContextFactory { get; }
        #endregion

        #region Constructor
        public Repository(IDataContextFactory dataContextFactory)
        {
            this.DataContextFactory = dataContextFactory;
        }
        #endregion

        #region Methods
        public void Dispose()
		{
		}
		#endregion
	}
}
