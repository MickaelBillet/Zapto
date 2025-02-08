using AirZapto.Data.DataContext;
using Framework.Data.Abstractions;

namespace AirZapto.Data.Repositories
{
    public abstract class Repository
	{
		#region Properties
		protected AirZaptoContext? DataContext { get; set; }
		#endregion

		#region Constructor
		public Repository(IDalSession session)
		{
            this.DataContext = session.DataContext as AirZaptoContext;
        }
        #endregion

        #region Methods
        public void Dispose()
		{
			this.DataContext?.Dispose();
		}
		#endregion
	}
}
