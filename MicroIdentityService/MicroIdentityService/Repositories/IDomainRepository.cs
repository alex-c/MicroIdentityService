using MicroIdentityService.Models;
using System;
using System.Collections.Generic;

namespace MicroIdentityService.Repositories
{
    /// <summary>
    /// An abstract interface for a repository of domains.
    /// </summary>
    public interface IDomainRepository
    {
        /// <summary>
        /// Gets all available domains.
        /// </summary>
        /// <returns>Returns all domains.</returns>
        IEnumerable<Domain> GetDomains();

        /// <summary>
        /// Search domains by name. All domains containing the search string in the name (case insensitive) are returned.
        /// </summary>
        /// <returns>Returns all matching domains.</returns>
        IEnumerable<Domain> SearchDomainsByName(string filter);

        /// <summary>
        /// Gets a domain by its unique ID.
        /// </summary>
        /// <param name="id">ID of the domain to get.</param>
        /// <returns>Returns the domain or null.</returns>
        Domain GetDomain(Guid id);

        /// <summary>
        /// Gets a domain by its unique name.
        /// </summary>
        /// <param name="name">The name of the domain to get.</param>
        /// <returns>Returns the domain or null.</returns>
        Domain GetDomain(string name);

        /// <summary>
        /// Creates a new domain.
        /// </summary>
        /// <param name="name">The name of the domain to create.</param>
        /// <returns>Returns the newly created domain.</returns>
        Domain CreateDomain(string name);

        /// <summary>
        /// Updates an existing domain.
        /// </summary>
        /// <param name="domain">The domain to update.</param>
        /// <returns>Returns the updated domain.</returns>
        Domain UpdateDomain(Domain domain);

        /// <summary>
        /// Deletes a domain. Deletion of a domain that doesn't exist is considered successful.
        /// </summary>
        /// <param name="id">The ID of the domain to delete.</param>
        void DeleteDomain(Guid id);
    }
}
