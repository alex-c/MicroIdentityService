using MicroIdentityService.Models;
using System;
using System.Collections.Generic;

namespace MicroIdentityService.Repositories
{
    /// <summary>
    /// An abstract interface for a repository of identity roles.
    /// </summary>
    public interface IRoleRepository
    {
        /// <summary>
        /// Gets all roles.
        /// </summary>
        /// <returns>Returns all roles.</returns>
        IEnumerable<Role> GetRoles();

        /// <summary>
        /// Gets all roles belonging to a given domain.
        /// </summary>
        /// <param name="domainId">ID of the domain to get roles for.</param>
        /// <returns>Returns all domain roles.</returns>
        IEnumerable<Role> GetDomainRoles(Guid domainId);

        /// <summary>
        /// Gets a role by its unique ID.
        /// </summary>
        /// <param name="id">ID of the role to get.</param>
        /// <returns>Returns the role or null.</returns>
        Role GetRole(Guid id);

        /// <summary>
        /// Gets a role by its unique name.
        /// </summary>
        /// <param name="name">Name of the role to get.</param>
        /// <param name="domainId">Domain for which to find the role.</param>
        /// <returns>Returns the role or null.</returns>
        Role GetRole(string name, Guid? domainId);

        /// <summary>
        /// Creates a new role, optionally belonging to a domain.
        /// </summary>
        /// <param name="name">Name of the role to create.</param>
        /// <param name="domainId">Optional ID of the domain to create the role for.</param>
        /// <returns>Returns the newly created role.</returns>
        Role CreateRole(string name, Guid? domainId);

        /// <summary>
        /// Updates an existing role.
        /// </summary>
        /// <param name="role">The role to update.</param>
        /// <returns>Returns the updated role.</returns>
        Role UpdateRole(Role role);

        /// <summary>
        /// Deletes a role if it exists.
        /// </summary>
        /// <param name="id">ID of the role to delete.</param>
        void DeleteRole(Guid id);
    }
}
