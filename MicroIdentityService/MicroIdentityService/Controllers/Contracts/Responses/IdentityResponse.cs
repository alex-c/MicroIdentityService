using MicroIdentityService.Models;
using System;

namespace MicroIdentityService.Controllers.Contracts.Responses
{
    /// <summary>
    /// A generic user contract for responses containing a user's identity.
    /// </summary>
    public class IdentityResponse
    {
        /// <summary>
        /// Creates an instance from an identity model.
        /// </summary>
        /// <param name="identity">The identity model for which to create an instance.</param>
        public IdentityResponse(Identity identity)
        {
            Id = identity.Id;
            Email = identity.Email;
        }

        /// <summary>
        /// The user's unique ID.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The user's email address.
        /// </summary>
        public string Email { get; set; }
    }
}
