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
    /// has the MIS administrator role, in which case the required permission is ignored.
    /// </summary>
    public class AdministratorAuthorizationHandler : AuthorizationHandler<PermissionAuthorizationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionAuthorizationRequirement requirement)
        {
            IEnumerable<Claim> roles = context.User.Claims.Where(c => c.Type == ClaimTypes.Role);

            // Check for administrator role
            if (roles.Select(r => r.Value).Contains(MisConstants.MIS_ADMINISTRATOR_ROLE))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
