using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MicroIdentityService.Authorization
{
    /// <summary>
    /// An authorization handler for endpoints protected by a <see cref="PermissionAuthorizationRequirement"/>. This checks that the caller
    /// has the required permission.
    /// </summary>
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionAuthorizationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionAuthorizationRequirement requirement)
        {
            IEnumerable<Claim> permissions = context.User.Claims.Where(c => c.Type == MisConstants.JWT_PERMISSIONS);

            // Check for the required permission
            if (permissions != null && permissions.Select(p => p.Value).Contains(requirement.Permission))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}