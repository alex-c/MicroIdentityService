using MicroIdentityService.Models;
using System;
using System.Collections.Generic;

namespace MicroIdentityService.Repositories
{
    /// <summary>
    /// An interface for a repository of identities, that grants read-only access.
    /// </summary>
    public interface IReadOnlyIdentityRepository
    {
        /// <summary>
        /// Gets all available identities.
        /// </summary>
        /// <returns>Returns all identities.</returns>
        IEnumerable<Identity> GetIdentities();

        /// <summary>
        /// Gets an identity from its unique ID. Returns null if no matching identity could be found.
        /// </summary>
        /// <param name="id">The ID of the identity to retrieve.</param>
        /// <returns>Returns the identity or null.</returns>
        Identity GetIdentity(Guid id);

        /// <summary>
        /// Gets an identity from its unique email address. Returns null if no matching identity could be found.
        /// </summary>
        /// <param name="email">The email address of the identity to retrieve.</param>
        /// <returns>Returns the identity or null.</returns>
        Identity GetIdentity(string email);
    }
}
