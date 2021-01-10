using System.Collections.Generic;

namespace MicroIdentityService.Controllers.Contracts.Requests
{
    /// <summary>
    /// A contract for a response containing a list of permissions.
    /// </summary>
    public class ApiKeyPermissionsUpdateRequest
    {
        /// <summary>
        /// The list of permissions to transmit.
        /// </summary>
        public IEnumerable<string> Permissions { get; set; }
    }
}
