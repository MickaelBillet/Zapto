﻿using AirZapto.Data.Entities;
using AirZapto.Data.Services;
using AirZapto.Data.Services.Repositories;
using Framework.Core.Base;
using Framework.Data.Abstractions;

namespace AirZapto.Data.Supervisors
{
    public sealed class SupervisorVersion : Supervisor, ISupervisorVersion
    {
        private readonly Lazy<IVersionRepository>? _lazyVersionRepository;

        #region Properties
        private IVersionRepository VersionRepository => _lazyVersionRepository!.Value;
        #endregion

        #region Constructor
        public SupervisorVersion(IDalSession session, IRepositoryFactory repositoryFactory) : base()
        {
            _lazyVersionRepository = repositoryFactory.CreateVersionRepository(session);
        }
        #endregion

        #region Methods
        public async Task<Version> GetVersionAsync()
        {
            VersionEntity? entity = null;
            if (this.VersionRepository != null)
            {

                entity = await this.VersionRepository.GetVersionAsync();
            }
            return (entity != null) ? new Version(entity.Major, entity.Minor, entity.Build) : new Version(0,0,0);
        }

        public async Task<ResultCode> AddVersionAsync(int major, int minor, int build)
        {
            ResultCode result = ResultCode.CouldNotCreateItem;
            if (this.VersionRepository != null)
            {
                VersionEntity? entity = await this.VersionRepository.GetVersionAsync();
                if (entity == null)
                {
                    bool res = await this.VersionRepository.AddVersionAsync(new VersionEntity()
                    {
                        Id = Guid.NewGuid().ToString(),
                        CreationDateTime = Clock.Now,
                        Major = major,
                        Minor = minor,
                        Build = build,
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
            if (this.VersionRepository != null)
            {
                VersionEntity? entity = await this.VersionRepository.GetVersionAsync();
                if (entity != null)
                {
                    bool res = await this.VersionRepository.UpdateVersionAsync(new VersionEntity()
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
