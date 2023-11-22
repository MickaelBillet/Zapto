using Connect.Data.Entities;
using Connect.Data.Services.Repositories;
using Framework.Core.Base;
using Framework.Data.Abstractions;

namespace Connect.Data.Supervisors
{
    public sealed class SupervisorVersion : ISupervisorVersion
    {
        private readonly Lazy<IRepository<VersionEntity>> _lazyVersionRepository;

        #region Properties
        private IRepository<VersionEntity> VersionRepository => _lazyVersionRepository.Value;
        #endregion

        #region Constructor
        public SupervisorVersion(IDalSession session, IRepositoryFactory repositoryFactory)
        {
            _lazyVersionRepository = repositoryFactory.CreateRepository<VersionEntity>(session);

        }
        #endregion

        #region Methods
        public async Task<Version> GetVersion()
        {
            VersionEntity entity = (await this.VersionRepository.GetCollectionAsync()).FirstOrDefault();
            return (entity != null) ? new Version(entity.Major, entity.Minor, entity.Build) : new Version(0,0,0);
        }

        public async Task<ResultCode> AddVersion()
        {
            ResultCode result = ResultCode.CouldNotCreateItem;
            VersionEntity entity = (await this.VersionRepository.GetCollectionAsync()).FirstOrDefault();
            if (entity == null)
            {
                int res = await this.VersionRepository.InsertAsync(new VersionEntity()
                {
                    Id = Guid.NewGuid().ToString(),
                    CreationDateTime = Clock.Now,
                    Major = 0,
                    Minor = 0,
                    Build = 0,
                }); ;
                
                result = (res > 0) ? ResultCode.Ok : ResultCode.CouldNotCreateItem;
            }
            else
            {
                result = ResultCode.ItemAlreadyExist;
            }

            return result;
        }

        public async Task<ResultCode> UpdateVersion(int major, int minor, int build)
        {
            ResultCode result = ResultCode.CouldNotUpdateItem;
            VersionEntity entity = (await this.VersionRepository.GetCollectionAsync()).FirstOrDefault();
            if (entity != null)
            {
                int res = await this.VersionRepository.UpdateAsync(new VersionEntity()
                {
                    Id = entity.Id,
                    CreationDateTime = Clock.Now,
                    Major = major,
                    Minor = minor,
                    Build = build,
                });
                result = (res > 0) ? ResultCode.Ok : ResultCode.CouldNotUpdateItem;
            }
            else
            {
                result = ResultCode.ItemNotFound;
            }

            return result;
        }
        #endregion
    }
}
