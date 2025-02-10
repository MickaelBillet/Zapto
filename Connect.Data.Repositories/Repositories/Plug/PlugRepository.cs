using Connect.Data.Entities;
using Connect.Data.Services.Repositories;
using Framework.Data.Abstractions;

namespace Connect.Data.Repositories
{
    public abstract class PlugRepository : Repository<PlugEntity>, IPlugRepository
    {
        #region Constructor
        public PlugRepository(IDataContextFactory dataContextFactory) : base(dataContextFactory) { }
        #endregion

        #region Methods
        public abstract Task<int> Upgrade1_1();
        #endregion
    }
}
