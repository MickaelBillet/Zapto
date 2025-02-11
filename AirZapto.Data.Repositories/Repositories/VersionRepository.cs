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
        public VersionRepository(IDataContextFactory dataContextFactory) : base(dataContextFactory) { }
        #endregion

        #region Methods
        public async Task<VersionEntity?> GetVersionAsync()
		{
			VersionEntity? versionEntity = null;
			await this.DataContextFactory.UseContext(async (context) =>
			{
				if (context != null)
				{
					versionEntity = await (from version in context.Set<VersionEntity>()
										   select version).AsNoTracking().FirstOrDefaultAsync();
				}
			});

			return versionEntity;
		}

		public async Task<bool> AddVersionAsync(VersionEntity entity)
		{
			bool res = false;
			await this.DataContextFactory.UseContext(async (context) =>
			{
                if (context != null)
                {
					await context.Set<VersionEntity>().AddAsync(entity);
					res = (await context.SaveChangesAsync() > 0) ? true : false;
				}
			});

			return res;
		}

		public async Task<bool> UpdateVersionAsync(VersionEntity entity)
		{
			bool res = false;
			await this.DataContextFactory.UseContext(async (context) =>
			{
				if (context != null)
				{
					context.DetachLocal<VersionEntity>(entity, entity.Id);
					res = (await context.SaveChangesAsync() > 0) ? true : false;
				}
			});

			return res;
		}

		#endregion
	}
}
