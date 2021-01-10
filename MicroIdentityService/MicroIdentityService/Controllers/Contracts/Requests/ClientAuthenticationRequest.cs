using System;

namespace MicroIdentityService.Controllers.Contracts.Requests
{
    /// <summary>
    /// A contract for an API key-based client authentication request.
    /// </summary>
    public class ClientAuthenticationRequest
    {
        /// <summary>
        /// Client application API key.
        /// </summary>
        public Guid ApiKey { get; set; }
    }
}
