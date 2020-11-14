using MicroIdentityService.Models;
using System;
using System.Collections.Generic;

namespace MicroIdentityService.Repositories
{
    /// <summary>
    /// A generic interface for a repository of identities.
    /// </summary>
    public interface IIdentityRepository
    {
        /// <summary>
        /// Gets all available identities.
        /// </summary>
        /// <returns>Returns all identities.</returns>
        IEnumerable<Identity> GetIdentities();

        /// <summary>
        /// Searches identities by identifier. All identities containing the search string in the identifier (case insensitive) are returned.
        /// </summary>
        /// <param name="filter">A string to filter identity identifiers with.</param>
        /// <returns>Returns all matching identities.</returns>
        IEnumerable<Identity> SearchIdentitiesByIdentifier(string filter);

        /// <summary>
        /// Gets an identity by its unique ID. Returns null if no matching identity could be found.
        /// </summary>
        /// <param name="id">The ID of the identity to retrieve.</param>
        /// <returns>Returns the identity or null.</returns>
        Identity GetIdentity(Guid id);

        /// <summary>
        /// Gets an identity by its unique user-chosen identifier. Returns null if no matching identity could be found.
        /// </summary>
        /// <param name="identifier">The identifier of the identity to retrieve.</param>
        /// <returns>Returns the identity or null.</returns>
        Identity GetIdentity(string identifier);

        /// <summary>
        /// Creates a new identity.
        /// </summary>
        /// <param name="identifier">The user-chosen identifier of the new identity to create.</param>
        /// <param name="hashedPassword">The hashed password of the new identity to create.</param>
        /// <param name="salt">The salt used to hash the password of the new identity to create.</param>
        /// <returns>Returns the newly created identity.</returns>
        Identity CreateIdentity(string identifier, string hashedPassword, byte[] salt);

        /// <summary>
        /// Updates an existing identity.
        /// </summary>
        /// <param name="identity">The identity to update.</param>
        /// <returns>Returns the updated identity</returns>
        Identity UpdateIdentity(Identity identity);

        /// <summary>
        /// Deletes an existing identity.
        /// </summary>
        /// <param name="id">The ID of the identity to delete.</param>
        void DeleteIdentity(Guid id);
    }
}
