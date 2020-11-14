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
        /// Searches identities by identifier. All identities containing the search string in the identifier (case insensitive) are returned.
        /// </summary>
        /// <param name="filter">A string to filter identity identifiers with.</param>
        /// <returns>Returns all matching identities.</returns>
        IEnumerable<Identity> SearchIdentitiesByIdentifier(string filter);

        /// <summary>
        /// Gets an identity from its unique ID. Returns null if no matching identity could be found.
        /// </summary>
        /// <param name="id">The ID of the identity to retrieve.</param>
        /// <returns>Returns the identity or null.</returns>
        Identity GetIdentity(Guid id);

        /// <summary>
        /// Gets an identity by its unique unser-chosen identifier. Returns null if no matching identity could be found.
        /// </summary>
        /// <param name="identifier">The identifier of the identity to retrieve.</param>
        /// <returns>Returns the identity or null.</returns>
        Identity GetIdentity(string identifier);
    }
}
