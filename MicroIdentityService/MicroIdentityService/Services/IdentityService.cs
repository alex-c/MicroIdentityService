using MicroIdentityService.Models;
using MicroIdentityService.Repositories;
using MicroIdentityService.Services.Exceptions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace MicroIdentityService.Services
{
    /// <summary>
    /// Provides business logic relating to the management of identities.
    /// </summary>
    public class IdentityService
    {
        /// <summary>
        /// Provides password hashing functionality.
        /// </summary>
        private PasswordHashingService PasswordHashingService { get; }

        /// <summary>
        /// A repository of identities.
        /// </summary>
        private IIdentityRepository IdentityRepository { get; }

        /// <summary>
        /// A logger for local logging needs.
        /// </summary>
        private ILogger Logger { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityService"/> class.
        /// </summary>
        /// <param name="loggerFactory">A factory to create loggers from.</param>
        /// <param name="passwordHashingService">The password hashing service.</param>
        /// <param name="identityRepository">A repository of identities.</param>
        public IdentityService(ILoggerFactory loggerFactory, 
            PasswordHashingService passwordHashingService,
            IIdentityRepository identityRepository)
        {
            Logger = loggerFactory.CreateLogger<IdentityService>();
            PasswordHashingService = passwordHashingService;
            IdentityRepository = identityRepository;
        }

        /// <summary>
        /// Gets all identities.
        /// </summary>
        /// <returns>Returns all identities.</returns>
        public IEnumerable<Identity> GetIdentities()
        {
            return IdentityRepository.GetIdentities();
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
        /// Creates a new identity.
        /// </summary>
        /// <param name="identifier">The unique user-chosen identifier with this identity.</param>
        /// <param name="password">The as-of-yet unhashed password of this identity.</param>
        /// <returns></returns>
        /// <exception cref="EntityAlreadyExsistsException">Identity</exception>
        public Identity CreateIdentity(string identifier, string password)
        {
            if (IdentityRepository.GetIdentity(identifier) != null)
            {
                throw new EntityAlreadyExsistsException("Identity", identifier);
            }

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
            IdentityRepository.UpdateIdentity(identity);
            return identity;
        }

        /// <summary>
        /// Deletes an identity.
        /// </summary>
        /// <param name="id">The ID of the identity to delete.</param>
        public void DeleteIdentity(Guid id)
        {
            IdentityRepository.DeleteIdentity(id);
        }
    }
}
