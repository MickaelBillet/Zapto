using AirZapto.Data.DataContext;
using AirZapto.Data.Entities;
using AirZapto.Data.Services.Repositories;
using Framework.Data.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AirZapto.Data.Repositories
{
    public class VersionRepository : Repository, IVersionRepository
	{
        #region Constructor
        public VersionRepository(IDalSession session) : base(session)
        { }
        #endregion

        #region Methods
        public async Task<VersionEntity?> GetVersionAsync()
		{
			VersionEntity? versionEntity = null;
			if (this.DataContext != null)
			{
                versionEntity = await (from version in this.DataContext.VersionEntities
										select version).AsNoTracking().FirstOrDefaultAsync();
			}

			return versionEntity;
		}

		public async Task<bool> AddVersionAsync(VersionEntity entity)
		{
			bool res = false;

			if (this.DataContext != null)
			{
				await this.DataContext.VersionEntities.AddAsync(entity);
				res = (await this.DataContext.SaveChangesAsync() > 0) ? true : false;
			}

			return res;
		}

		public async Task<bool> UpdateVersionAsync(VersionEntity entity)
		{
			bool res = false;

			if (this.DataContext != null)
			{
                this.DataContext.DetachLocal<VersionEntity>(entity, entity.Id);
                res = (await this.DataContext.SaveChangesAsync() > 0) ? true : false;
			}

			return res;
		}

		#endregion
	}
}
