using MicroIdentityService.Models;
using System;
using System.Collections.Generic;

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
            Identifier = identity.Identifier;
            Disabled = identity.Disabled;
            Roles = identity.Roles;
        }

        /// <summary>
        /// The user's unique system ID.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The user-chosen unique identifier.
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// Whether the identity has been deisabled.
        /// </summary>
        public bool Disabled { get; set; }

        /// <summary>
        /// The roles associated with this identity.
        /// </summary>
        public IEnumerable<Role> Roles { get; set; }
    }
}
