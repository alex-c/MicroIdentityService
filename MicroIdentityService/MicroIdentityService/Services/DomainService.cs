﻿using MicroIdentityService.Models;
using MicroIdentityService.Repositories;
using MicroIdentityService.Services.Exceptions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroIdentityService.Services
{
    /// <summary>
    /// A service that manages domains.
    /// </summary>
    public class DomainService
    {
        /// <summary>
        /// A repository that provides domain persistence.
        /// </summary>
        private IDomainRepository DomainRepository { get; }

        /// <summary>
        /// A logger instance for local logging needs.
        /// </summary>
        private ILogger Logger { get; }

        /// <summary>
        /// Sets up the domain service with all needed dependencies.
        /// </summary>
        /// <param name="logger">A logger instance for local logging needs.</param>
        /// <param name="domainRepository">A repository that provides domains.</param>
        public DomainService(ILogger<DomainService> logger, IDomainRepository domainRepository)
        {
            DomainRepository = domainRepository;
            Logger = logger;
        }

        /// <summary>
        /// Gets domains, optionally filtered by name.
        /// </summary>
        /// <param name="filter">A string to filter domain names with.</param>
        /// <returns>Returns domains.</returns>
        public async Task<IEnumerable<Domain>> GetDomains(string filter = null)
        {
            if (filter == null)
            {
                return await DomainRepository.GetDomains();
            }
            else
            {
                return await DomainRepository.SearchDomainsByName(filter);
            }
        }

        /// <summary>
        /// Gets a domain by its unique ID.
        /// </summary>
        /// <param name="id">ID of the domain to get.</param>
        /// <returns>Returns the domain.</returns>
        /// <exception cref="EntityNotFoundException">Thrown if no matching domain could be found.</exception>
        public async Task<Domain> GetDomain(Guid id)
        {
            Domain domain = await DomainRepository.GetDomain(id);
            if (domain == null)
            {
                throw new EntityNotFoundException("Domain", id);
            }
            return domain;
        }

        /// <summary>
        /// Creates a new domain with a given name.
        /// </summary>
        /// <param name="name">The name of the domain to create.</param>
        /// <returns>Returns the newly created domain.</returns>
        /// <exception cref="EntityAlreadyExsistsException">Thrown if the domain name is already taken.</exception>
        public async Task<Domain> CreateDomain(string name)
        {
            if (await DomainRepository.GetDomain(name) != null)
            {
                throw new EntityAlreadyExsistsException("Domain", name);
            }
            return await DomainRepository.CreateDomain(name);
        }

        /// <summary>
        /// Updates an existing domain.
        /// </summary>
        /// <param name="id">ID of the domain to update.</param>
        /// <param name="name">The new domain name to set.</param>
        /// <returns>Returns the updated domain.</returns>
        /// <exception cref="EntityNotFoundException">Thrown if no matching domain could be found.</exception>
        public async Task<Domain> UpdateDomain(Guid id, string name)
        {
            Domain domain = await GetDomain(id);
            domain.Name = name;
            return await DomainRepository.UpdateDomain(domain);
        }

        /// <summary>
        /// Deletes an identitiy. An attempt to delete a domain which doesn't exist is considered successful.
        /// </summary>
        /// <param name="id">ID of the domain to delete.</param>
        public async Task DeleteDomain(Guid id)
        {
            await DomainRepository.DeleteDomain(id);
        }
    }
}
