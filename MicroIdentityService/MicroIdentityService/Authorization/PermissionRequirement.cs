using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace MicroIdentityService.Authorization
{
    /// <summary>
    /// An authorization requirement that requires a single permission, or any of a number of roles.
    /// </summary>
    public class PermissionRequirement : IAuthorizationRequirement
    {
        /// <summary>
        /// Any of these roles suffice for authorization.
        /// </summary>
        public IEnumerable<string> Roles { get; }

        /// <summary>
        /// The permission needed for authorization.
        /// </summary>
        public string Permission { get; }

        /// <summary>
        /// Initializes the requirement with any number of roles or a permission or both.
        /// </summary>
        /// <param name="roles">Roles to add to the requirement.</param>
        /// <param name="permission">Permissions to add to the requirement.</param>
        public PermissionRequirement(IEnumerable<string> roles, string permission)
        {
            Roles = roles;
            Permission = permission;
        }
    }
}
