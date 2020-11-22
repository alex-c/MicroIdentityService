using MicroIdentityService.Models;
using System;
using System.Collections.Generic;

namespace MicroIdentityService.Repositories
{
    /// <summary>
    /// A repository for identity role mappings.
    /// </summary>
    public interface IIdentityRoleRepository
    {
        /// <summary>
        /// Sets identity roles. This removes previous identity roles and sets the new ones.
        /// </summary>
        /// <param name="identity">Identity for which to set roles.</param>
        /// <param name="roles">Roles to set.</param>
        void SetIdentityRoles(Identity identity, IEnumerable<Role> roles);
    }
}
