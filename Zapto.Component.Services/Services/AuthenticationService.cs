using Framework.Core.Domain;
using Framework.Infrastructure.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Security.Claims;

namespace Zapto.Component.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        #region Properties
        private AuthenticationStateProvider AuthenticationStateProvider { get; }
        #endregion

        #region Constructor
        public AuthenticationService(IServiceProvider serviceProvider) 
        {
            this.AuthenticationStateProvider = serviceProvider.GetRequiredService<AuthenticationStateProvider>();
        }
        #endregion

        #region Methods
        private async Task<string?> GetClaim(string name)
        {
            string? value = null;
            AuthenticationState authState = await this.AuthenticationStateProvider.GetAuthenticationStateAsync();
            ClaimsPrincipal user = authState.User;

            if (user.Identity?.IsAuthenticated == true)
            {
                value = user.Claims.FirstOrDefault(claim => claim.Type == name)?.Value;
            }

            return value;
        }

        public async Task<ZaptoUser?> GetAuthenticatedUser()
        {
            ZaptoUser? user = new ZaptoUser();

            AuthenticationState authState = await this.AuthenticationStateProvider.GetAuthenticationStateAsync();
            ClaimsPrincipal principal = authState.User;

            if (principal.Identity?.IsAuthenticated == true)
            {
#if DEBUG
                foreach (var claim in principal.Claims) 
                {
                    Log.Information(claim.Type + " :" + claim.Value);
                }
#endif

                user.LocationName = principal.Claims.FirstOrDefault(claim => claim.Type == "location")?.Value;
                user.LocationLatitude = principal.Claims.FirstOrDefault(claim => claim.Type == "latitude")?.Value;
                user.LocationLongitude = principal.Claims.FirstOrDefault(claim => claim.Type == "longitude")?.Value;
                user.Firstname = principal.Claims.FirstOrDefault(claim => claim.Type == "given_name")?.Value;
                user.Lastname = principal.Claims.FirstOrDefault(claim => claim.Type == "family_name")?.Value;
                user.Username = principal.Claims.FirstOrDefault(claim => claim.Type == "preferred_username")?.Value;
            }

            return user;
        }
        #endregion
    }
}
