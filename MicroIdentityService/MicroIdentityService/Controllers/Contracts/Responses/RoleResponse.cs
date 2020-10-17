using MicroIdentityService.Models;
using System;

namespace MicroIdentityService.Controllers.Contracts.Responses
{
    /// <summary>
    /// A contract for a response that contains a role object.
    /// </summary>
    public class RoleResponse
    {
        /// <summary>
        /// Unique ID of the role.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Unique name of the role.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The optional domain that this roles belongs to.
        /// </summary>
        public Guid? DomainId { get; set; }

        /// <summary>
        /// Creates a role response contract from a role entity.
        /// </summary>
        /// <param name="role"></param>
        public RoleResponse(Role role)
        {
            Id = role.Id;
            Name = role.Name;
            DomainId = role.DomainId;
        }
    }
}
