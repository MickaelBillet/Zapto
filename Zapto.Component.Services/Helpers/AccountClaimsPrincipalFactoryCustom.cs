using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Security.Claims;
using System.Text.Json;

namespace Zapto.Component.Services.Helpers
{
    public class AccountClaimsPrincipalFactoryCustom : AccountClaimsPrincipalFactory<RemoteUserAccount>
    {
        public AccountClaimsPrincipalFactoryCustom(IAccessTokenProviderAccessor accessor) : base(accessor)
        {
        }

        public async override ValueTask<ClaimsPrincipal> CreateUserAsync(RemoteUserAccount account, RemoteAuthenticationUserOptions options)
        {
            var user = await base.CreateUserAsync(account, options);
            if (user.Identity?.IsAuthenticated != true)
            {
                return user;
            }

            var identity = (ClaimsIdentity)user.Identity;
            var roleClaims = identity.FindAll(identity.RoleClaimType);

            if (roleClaims == null || !roleClaims.Any())
            {
                return user;
            }

            var rolesElem = account.AdditionalProperties[identity.RoleClaimType];

            if (rolesElem is JsonElement roles)
            {
                if ((roles.ValueKind == JsonValueKind.Array) && (options.RoleClaim != null))
                {
                    identity.RemoveClaim(identity.FindFirst(options.RoleClaim));
                    foreach (var role in roles.EnumerateArray())
                    {
                        identity.AddClaim(new Claim(options.RoleClaim, role.ToString() ?? string.Empty));
                    }
                }
            }

            return user;
        }
    }
}
