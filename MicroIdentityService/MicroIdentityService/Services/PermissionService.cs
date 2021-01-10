using MicroIdentityService.Authorization;
using MicroIdentityService.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MicroIdentityService.Services
{
    /// <summary>
    /// A service for business logic related to MIS permissions.
    /// </summary>
    public class PermissionService
    {
        /// <summary>
        /// Holds all available MIS permissions.
        /// </summary>
        private IEnumerable<string> AllPermissions { get; }

        /// <summary>
        /// Sets up the repository with a list of all available MIS permissions.
        /// </summary>
        public PermissionService()
        {
            AllPermissions = new List<string>()
            {
                Permissions.API_KEYS_CREATE,
                Permissions.API_KEYS_DELETE,
                Permissions.API_KEYS_GET,
                Permissions.API_KEYS_UPDATE,
                Permissions.DOMAINS_CREATE,
                Permissions.DOMAINS_DELETE,
                Permissions.DOMAINS_GET,
                Permissions.DOMAINS_UPDATE,
                Permissions.IDENTITIES_CREATE,
                Permissions.IDENTITIES_DELETE,
                Permissions.IDENTITIES_GET,
                Permissions.IDENTITIES_GET_ROLES,
                Permissions.IDENTITIES_SET_ROLES,
                Permissions.IDENTITIES_UPDATE,
                Permissions.ROLES_CREATE,
                Permissions.ROLES_DELETE,
                Permissions.ROLES_GET,
                Permissions.ROLES_UPDATE
            };
        }

        /// <summary>
        /// Gets the list of all available MIS permissions.
        /// </summary>
        /// <returns>Returns all permissions.</returns>
        public IEnumerable<string> GetAllPermissions()
        {
            return AllPermissions;
        }

        /// <summary>
        /// Validates that all permissions in the provided list are existing MIS permissions.
        /// </summary>
        /// <param name="permissions">The permissiosn to validate.</param>
        /// <returns>Returns true on success.</returns>
        /// <exception cref="EntityNotFoundException">Thrown for the first permission that could not be validated.</exception>
        public bool ValidatePermissions(IEnumerable<string> permissions)
        {
            foreach (string permission in permissions)
            {
                if (!AllPermissions.Contains(permission))
                {
                    throw new EntityNotFoundException("Permission", permission);
                }
            }
            return true;
        }
    }
}
