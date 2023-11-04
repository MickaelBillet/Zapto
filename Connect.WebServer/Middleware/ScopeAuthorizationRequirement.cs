using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Connect.WebApi.Middleware
{
    public class ScopeAuthorizationRequirement : AuthorizationHandler<ScopeAuthorizationRequirement>, IAuthorizationRequirement
    {
        public IEnumerable<string> RequiredScopes { get; }

        public ScopeAuthorizationRequirement(IEnumerable<string> requiredScopes)
        {
            if (requiredScopes == null || !requiredScopes.Any())
            {
                throw new ArgumentException($"{nameof(requiredScopes)} must contain at least one value.", nameof(requiredScopes));
            }

            RequiredScopes = requiredScopes;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ScopeAuthorizationRequirement requirement)
        {
            if (context.User != null)
            {
                IEnumerable<Claim> scopeClaims = context.User.Claims.Where(c => string.Equals(c.Type, "scope", StringComparison.OrdinalIgnoreCase));
                if (scopeClaims != null)
                {
                    foreach (Claim claim in scopeClaims)
                    {
                        string[] scopes = claim.Value.Split("-", StringSplitOptions.RemoveEmptyEntries);

                        //Required Scope for the controller
                        if (requirement.RequiredScopes.All(requiredScope => scopes.Contains(requiredScope)))
                        {
                            context.Succeed(requirement);
                            break;
                        }
                    }
                }
            }

            return Task.CompletedTask;
        }
    }
}
