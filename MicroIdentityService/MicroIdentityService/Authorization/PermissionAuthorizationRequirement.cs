using Microsoft.AspNetCore.Authorization;

namespace MicroIdentityService.Authorization
{
    /// <summary>
    /// An authorization requirement that requires a single specific permission.
    /// </summary>
    public class PermissionAuthorizationRequirement : IAuthorizationRequirement
    {
        /// <summary>
        /// The permission needed for authorization.
        /// </summary>
        public string Permission { get; }

        /// <summary>
        /// Initializes the requirement with a specific permission.
        /// </summary>
        /// <param name="permission">The permission that is required for authorization.</param>
        public PermissionAuthorizationRequirement(string permission)
        {
            Permission = permission;
        }
    }
}
