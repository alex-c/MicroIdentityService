using MicroIdentityService.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        Task<IEnumerable<Domain>> GetDomains();

        /// <summary>
        /// Searches domains by name. All domains containing the search string in the name (case insensitive) are returned.
        /// </summary>
        /// <param name="filter">A string to filter domain names with.</param>
        /// <returns>Returns all matching domains.</returns>
        Task<IEnumerable<Domain>> SearchDomainsByName(string filter);

        /// <summary>
        /// Gets a domain by its unique ID.
        /// </summary>
        /// <param name="id">ID of the domain to get.</param>
        /// <returns>Returns the domain or null.</returns>
        Task<Domain> GetDomain(Guid id);

        /// <summary>
        /// Gets a domain by its unique name.
        /// </summary>
        /// <param name="name">The name of the domain to get.</param>
        /// <returns>Returns the domain or null.</returns>
        Task<Domain> GetDomain(string name);

        /// <summary>
        /// Creates a new domain.
        /// </summary>
        /// <param name="name">The name of the domain to create.</param>
        /// <returns>Returns the newly created domain.</returns>
        Task<Domain> CreateDomain(string name);

        /// <summary>
        /// Updates an existing domain.
        /// </summary>
        /// <param name="domain">The domain to update.</param>
        /// <returns>Returns the updated domain.</returns>
        Task<Domain> UpdateDomain(Domain domain);

        /// <summary>
        /// Deletes a domain. Deletion of a domain that doesn't exist is considered successful.
        /// </summary>
        /// <param name="id">The ID of the domain to delete.</param>
        Task DeleteDomain(Guid id);
    }
}
