﻿using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace Connect.WebApi.Middleware
{
	public static class ScopeAuthorizationRequirementExtensions
    {
        public static AuthorizationPolicyBuilder RequireScope(this AuthorizationPolicyBuilder authorizationPolicyBuilder, params string[] requiredScopes)
        {
            authorizationPolicyBuilder.RequireScope((IEnumerable<string>)requiredScopes);
            return authorizationPolicyBuilder;
        }

        public static AuthorizationPolicyBuilder RequireScope(this AuthorizationPolicyBuilder authorizationPolicyBuilder, IEnumerable<string> requiredScopes)
        {
            authorizationPolicyBuilder.AddRequirements(new ScopeAuthorizationRequirement(requiredScopes));
            return authorizationPolicyBuilder;
        }
    }
}
