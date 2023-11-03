using AirZapto.Data.DataContext;
using AirZapto.Data.Services.Repositories;

namespace AirZapto.Data.Repositories
{
    public partial class Repository : IRepository
	{
		#region Properties

		private AirZaptoContext? DataContext { get; }

		#endregion

		#region Constructor

		public Repository(AirZaptoContext? dataContext)
		{
			this.DataContext = dataContext as AirZaptoContext;
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
