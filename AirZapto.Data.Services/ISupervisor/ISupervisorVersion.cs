﻿using Framework.Core.Base;

namespace AirZapto.Data.Services
{
    public interface ISupervisorVersion
	{
        #region Version
        Task<Version> GetVersionAsync();
        Task<ResultCode> AddVersionAsync();
        Task<ResultCode> UpdateVersionAsync(int major, int minor, int build);
        #endregion
    }
}
