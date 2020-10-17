using MicroIdentityService.Models;
using MicroIdentityService.Repositories;
using MicroIdentityService.Services.Exceptions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace MicroIdentityService.Services
{
    /// <summary>
    /// Provides roles and role management funcionality.
    /// </summary>
    public class RoleService
    {
        /// <summary>
        /// A repository of identity roles.
        /// </summary>
        private IRoleRepository RoleRepository { get; }

        /// <summary>
        /// Provides access to domains.
        /// </summary>
        private DomainService DomainService { get; }

        /// <summary>
        /// A logger instance for local logging needs.
        /// </summary>
        private ILogger Logger { get; }

        /// <summary>
        /// Sets up the service with all needed dependencies.
        /// </summary>
        /// <param name="logger">A logger for local logging needs.</param>
        /// <param name="roleRepository">A repository of identity roles.</param>
        /// <param name="domainService">Provides domains.</param>
        public RoleService(ILogger<RoleService> logger, IRoleRepository roleRepository, DomainService domainService)
        {
            RoleRepository = roleRepository;
            DomainService = domainService;
            Logger = logger;
        }

        /// <summary>
        /// Gets identity roles, optionally filtered to the roles that belong to a given domain.
        /// </summary>
        /// <param name="domainId">ID of the domain to get identity roles for.</param>
        /// <returns>Returns a list of roles.</returns>
        /// <exception cref="EntityNotFoundException">Thrown if a domain ID was provided but no matching domain could be found.</exception>
        public IEnumerable<Role> GetRoles(Guid? domainId)
        {
            if (domainId == null)
            {
                return RoleRepository.GetRoles();
            }
            else
            {
                DomainService.GetDomain(domainId.Value); // Throws an EntityNotFoundException if domain doesn't exist
                return RoleRepository.GetDomainRoles(domainId.Value);
            }
        }

        /// <summary>
        /// Gets a role by its unique ID.
        /// </summary>
        /// <param name="id">ID of the role to get.</param>
        /// <returns>Returns the role.</returns>
        /// <exception cref="EntityNotFoundException">Thrown if the role could not be found.</exception>
        public Role GetRole(Guid id)
        {
            Role role = RoleRepository.GetRole(id);
            if (role == null)
            {
                throw new EntityNotFoundException("Role", id);
            }
            return role;
        }

        /// <summary>
        /// Creates a new role, optionally belonging to a given domain.
        /// </summary>
        /// <param name="name">Name of the role to create.</param>
        /// <param name="domainId">Optiona ID of the domain to create the role for.</param>
        /// <returns>Returns the newly created role.</returns>
        /// <exception cref="EntityAlreadyExsistsException">Thrown if the role name is already taken.</exception>
        public Role CreateRole(string name, Guid? domainId)
        {
            Role role = RoleRepository.GetRole(name, domainId);
            if (role != null)
            {
                throw new EntityAlreadyExsistsException("Role", name);
            }
            return RoleRepository.CreateRole(name, domainId);
        }

        /// <summary>
        /// Updates an existing role.
        /// </summary>
        /// <param name="id">The ID of the role to update.</param>
        /// <param name="name">The new role name to set.</param>
        /// <returns>Returns the updated role.</returns>
        /// <exception cref="EntityNotFoundException">Thrown if the role could not be found.</exception>
        public Role UpdateRole(Guid id, string name)
        {
            Role role = GetRole(id);
            role.Name = name;
            return RoleRepository.UpdateRole(role);
        }

        /// <summary>
        /// Deletes a role. A deletion attempt of a non-existing role is considered successful.
        /// </summary>
        /// <param name="id">ID of the role to delete.</param>
        public void DeleteRole(Guid id)
        {
            RoleRepository.DeleteRole(id);
        }
    }
}
