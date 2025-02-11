using AirZapto.Data.Entities;

namespace AirZapto.Data.Services.Repositories
{
    public interface IVersionRepository : IDisposable
	{
		#region Version
		Task<VersionEntity?> GetVersionAsync();
		Task<bool> AddVersionAsync(VersionEntity entity);
		Task<bool> UpdateVersionAsync(VersionEntity entity);
        #endregion
    }
}
