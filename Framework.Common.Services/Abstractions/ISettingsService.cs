using System;

namespace Framework.Infrastructure.Services
{
    public interface ISettingsService
    {
        String KeychainService { get; }
        String Scope { get; }
        String BackEndUrl { get; }
        String UserId { get; }
    }
}
