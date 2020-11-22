using MicroIdentityService.Models;
using MicroIdentityService.Repositories;
using MicroIdentityService.Services.Exceptions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MicroIdentityService.Services
{
    /// <summary>
    /// Provides business logic relating to the management of identities.
    /// </summary>
    public class IdentityService
    {
        /// <summary>
        /// Grants access to roles
        /// </summary>
        private RoleService RoleService { get; }

        /// <summary>
        /// Provides validation logic for client-provided identifiers.
        /// </summary>
        private IdentifierValidationService IdentifierValidationService { get; }

        /// <summary>
        /// Provides validation logic for client-provided passwords.
        /// </summary>
        private PasswordValidationService PasswordValidationService { get; }

        /// <summary>
        /// Provides password hashing functionality.
        /// </summary>
        private PasswordHashingService PasswordHashingService { get; }

        /// <summary>
        /// A repository of identities.
        /// </summary>
        private IIdentityRepository IdentityRepository { get; }

        /// <summary>
        /// Grants direct access to identity role mappings.
        /// </summary>
        private IIdentityRoleRepository IdentityRoleRepository { get; }

        /// <summary>
        /// A logger for local logging needs.
        /// </summary>
        private ILogger Logger { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityService"/> class.
        /// </summary>
        /// <param name="loggerFactory">A factory to create loggers from.</param>
        /// <param name="identifierValidationService">Provides validation logic for client-provided identifiers.</param>
        /// <param name="passwordValidationService">Provides validation logic for client-provided passwords.</param>
        /// <param name="passwordHashingService">The password hashing service.</param>
        /// <param name="identityRepository">A repository of identities.</param>
        public IdentityService(ILoggerFactory loggerFactory,
            RoleService roleService,
            IdentifierValidationService identifierValidationService,
            PasswordValidationService passwordValidationService,
            PasswordHashingService passwordHashingService,
            IIdentityRepository identityRepository,
            IIdentityRoleRepository identityRoleRepository)
        {
            Logger = loggerFactory.CreateLogger<IdentityService>();
            IdentifierValidationService = identifierValidationService;
            PasswordValidationService = passwordValidationService;
            PasswordHashingService = passwordHashingService;
            IdentityRepository = identityRepository;
            IdentityRoleRepository = identityRoleRepository;
            RoleService = roleService;
        }

        /// <summary>
        /// Gets all identities, optionally filtered by name.
        /// </summary>
        /// <param name="filter">A string to filter identities names with.</param>
        /// <returns>Returns identities.</returns>
        public IEnumerable<Identity> GetIdentities(string filter)
        {
            if (filter == null)
            {
                return IdentityRepository.GetIdentities();
            }
            else
            {
                return IdentityRepository.SearchIdentitiesByIdentifier(filter);
            }
        }

        /// <summary>
        /// Gets an identity by it's unique ID.
        /// </summary>
        /// <param name="id">The ID of the identity to get.</param>
        /// <returns>Returns the identity.</returns>
        /// <exception cref="EntityNotFoundException">Thrown if the identity could not be found.</exception>
        public Identity GetIdentity(Guid id)
        {
            Identity identity = IdentityRepository.GetIdentity(id);
            if (identity == null)
            {
                throw new EntityNotFoundException("Identity", id);
            }
            return identity;
        }

        /// <summary>
        /// Gets an identity by its unique user-chosen identifier.
        /// </summary>
        /// <param name="identifier">The identifier of the identity to retrieve.</param>
        /// <returns>Returns the identity, if found.</returns>
        /// <exception cref="EntityNotFoundException">Thrown if the identity could not be found.</exception>
        public Identity GetIdentity(string identifier)
        {
            Identity identity = IdentityRepository.GetIdentity(identifier);
            if (identity == null)
            {
                throw new EntityNotFoundException("Identity", identifier);
            }
            return identity;
        }

        /// <summary>
        /// Creates a new identity.
        /// </summary>
        /// <param name="identifier">The unique user-chosen identifier with this identity.</param>
        /// <param name="password">The as-of-yet unhashed password of this identity.</param>
        /// <returns></returns>
        /// <exception cref="EntityAlreadyExsistsException">Identity</exception>
        public Identity CreateIdentity(string identifier, string password)
        {
            // Validate identifier format and availability
            IdentifierValidationService.Validate(identifier);
            if (IdentityRepository.GetIdentity(identifier) != null)
            {
                throw new EntityAlreadyExsistsException("Identity", identifier);
            }

            // Validate client-provided password
            PasswordValidationService.Validate(password);
            
            // Hash password and create new identity
            (string hash, byte[] salt) = PasswordHashingService.HashAndSaltPassword(password);
            return IdentityRepository.CreateIdentity(identifier, hash, salt);
        }

        /// <summary>
        /// Updates an existing identity.
        /// </summary>
        /// <param name="id">ID of the identity to update.</param>
        /// <param name="disabled">Whether the identity is to be disabled</param>
        /// <returns>Returns the updated identity.</returns>
        /// <exception cref="EntityNotFoundException">Thrown if the identity could not be found.</exception>
        public Identity UpdateIdentity(Guid id, bool disabled)
        {
            Identity identity = GetIdentity(id);
            identity.Disabled = disabled;
            return IdentityRepository.UpdateIdentity(identity);
        }

        /// <summary>
        /// Deletes an identity.
        /// </summary>
        /// <param name="id">The ID of the identity to delete.</param>
        public void DeleteIdentity(Guid id)
        {
            IdentityRepository.DeleteIdentity(id);
        }

        /// <summary>
        /// Gets roles assigned to a given identity.
        /// </summary>
        /// <param name="id">ID of the identity to get roles for.</param>
        /// <returns>Returns a list of identity roles.</returns>
        /// <exception cref="EntityNotFoundException">Thrown if the identity could not be found.</exception>
        public IEnumerable<Role> GetIdentityRoles(Guid id)
        {
            Identity identity = GetIdentity(id);
            return identity.Roles;
        }

        /// <summary>
        /// Sets the roles assigned to a given identity.
        /// </summary>
        /// <param name="id">ID of the identity for which to update roles.</param>
        /// <param name="roleIds">The IDs of the roles to set.</param>
        /// <exception cref="EntityNotFoundException">Thrown if the identity or any of the roles could not be found.</exception>
        public void UpdateIdentityRoles(Guid id, IEnumerable<Guid> roleIds)
        {
            Identity identity = GetIdentity(id);
            IEnumerable<Role> roles = RoleService.GetRoles(roleIds);
            if (roles.Count() != roleIds.Count())
            {
                Guid firstMissingId = roles.First(r => !roleIds.Contains(r.Id)).Id;
                throw new EntityNotFoundException("Role", firstMissingId);
            }
            IdentityRoleRepository.SetIdentityRoles(identity, roles);
        }
    }
}
