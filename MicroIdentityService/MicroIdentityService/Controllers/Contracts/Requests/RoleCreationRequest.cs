using System;

namespace MicroIdentityService.Controllers.Contracts.Requests
{
    /// <summary>
    /// A contract for a request to create a role.
    /// </summary>
    public class RoleCreationRequest
    {
        /// <summary>
        /// Name of the role to crate.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Optiona ID of the domain to crate the role for.
        /// </summary>
        public Guid? DomainId { get; set; }
    }
}
