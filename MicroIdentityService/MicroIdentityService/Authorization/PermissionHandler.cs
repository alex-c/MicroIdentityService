using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MicroIdentityService.Authorization
{
    /// <summary>
    /// An authorization handler for endpoints protected by a <see cref="PermissionRequirement"/>. This checks that the caller
    /// either has the target permission, or any of a number of roles that implicitely grant the target permission.
    /// </summary>
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            IEnumerable<Claim> roles = context.User.Claims.Where(c => c.Type == ClaimTypes.Role);
            IEnumerable<Claim> permissions = context.User.Claims.Where(c => c.Type == "permissions");

            // Attempt to authorize based on permission
            if (permissions != null && permissions.Select(p => p.Value).Contains(requirement.Permission))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            // Attempt to authorize based on roles
            if (roles != null)
            {
                foreach (Claim role in roles)
                {
                    if (requirement.Roles.Contains(role.Value))
                    {
                        context.Succeed(requirement);
                        break;
                    }
                }
            }
            return Task.CompletedTask;
        }
    }
}