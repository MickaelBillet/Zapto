using AirZapto.Data.Entities;
using Framework.Core.Base;

namespace AirZapto.Data.Supervisors
{
    public sealed class SupervisorVersion : Supervisor, ISupervisorVersion
    {
        #region Constructor
        public SupervisorVersion(IServiceProvider serviceProvider) : base(serviceProvider)
        { }
        #endregion

        #region Methods
        public async Task<Version> GetVersionAsync()
        {
            VersionEntity? entity = null;
            if (this.Repository != null)
            {

                entity = await this.Repository.GetVersionAsync();
            }
            return (entity != null) ? new Version(entity.Major, entity.Minor, entity.Build) : new Version(0,0,0);
        }

        public async Task<ResultCode> AddVersionAsync()
        {
            ResultCode result = ResultCode.CouldNotCreateItem;
            if (this.Repository != null)
            {
                VersionEntity? entity = await this.Repository.GetVersionAsync();
                if (entity == null)
                {
                    bool res = await this.Repository.AddVersionAsync(new VersionEntity()
                    {
                        Id = Guid.NewGuid().ToString(),
                        CreationDateTime = Clock.Now,
                        Major = 0,
                        Minor = 0,
                        Build = 0,
                    }); ;

                    result = (res == true) ? ResultCode.Ok : ResultCode.CouldNotCreateItem;
                }
                else
                {
                    result = ResultCode.ItemAlreadyExist;
                }
            }

            return result;
        }

        public async Task<ResultCode> UpdateVersionAsync(int major, int minor, int build)
        {
            ResultCode result = ResultCode.CouldNotUpdateItem;
            if (this.Repository != null)
            {
                VersionEntity? entity = await this.Repository.GetVersionAsync();
                if (entity != null)
                {
                    bool res = await this.Repository.UpdateVersionAsync(new VersionEntity()
                    {
                        Id = entity.Id,
                        CreationDateTime = Clock.Now,
                        Major = major,
                        Minor = minor,
                        Build = build,
                    });
                    result = (res == true) ? ResultCode.Ok : ResultCode.CouldNotUpdateItem;
                }
                else
                {
                    result = ResultCode.ItemNotFound;
                }
            }

            return result;
        }
        #endregion
    }
}
