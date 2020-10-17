using System;

namespace MicroIdentityService.Controllers.Contracts.Requests
{
    /// <summary>
    /// A contract for an identity authentication request.
    /// </summary>
    public class AuthenticationRequest
    {
        /// <summary>
        /// ID of the identity to authenticate.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Password of the identity.
        /// </summary>
        public string Password { get; set; }
    }
}
