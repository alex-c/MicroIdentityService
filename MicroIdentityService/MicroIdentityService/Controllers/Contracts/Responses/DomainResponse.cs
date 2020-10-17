using MicroIdentityService.Models;
using System;

namespace MicroIdentityService.Controllers.Contracts.Responses
{
    /// <summary>
    /// Contract for a response containing a domain object.
    /// </summary>
    public class DomainResponse
    {
        /// <summary>
        /// Unique ID of the domain.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Unique name of the domain
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Creates a domain response contract from a domain entity.
        /// </summary>
        /// <param name="domain">Domain entity to create a contract for.</param>
        public DomainResponse(Domain domain)
        {
            Id = domain.Id;
            Name = domain.Name;
        }
    }
}
