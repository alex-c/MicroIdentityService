using System;
using System.Collections.Generic;

namespace MicroIdentityService.Controllers.Contracts.Requests
{
    /// <summary>
    /// A contract for a request to update an identity's assigned roles.
    /// </summary>
    public class IdentityRolesUpdateRequest
    {
        /// <summary>
        /// The roles to assign to the identity.
        /// </summary>
        public IEnumerable<Guid> RoleIds { get; set; }
    }
}
