using AirZapto.Data.Services.Repositories;
using Framework.Data.Abstractions;

namespace AirZapto.Data.Supervisors
{
    public abstract class Supervisor
	{
        private readonly Lazy<IRepository>? _lazyRepository;

		#region Properties
        protected IRepository? Repository => _lazyRepository?.Value;
		#endregion

		#region Constructor
		public Supervisor(IDalSession session, IDataContextFactory contextFactory, IRepositoryFactory repositoryFactory) 
		{
            _lazyRepository = repositoryFactory?.CreateRepository(session, contextFactory);
        }
        #endregion
    }
}
