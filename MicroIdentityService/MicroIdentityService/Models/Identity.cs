using System;
using System.Collections.Generic;

namespace MicroIdentityService.Models
{
    /// <summary>
    /// Represents an identity.
    /// </summary>
    public class Identity
    {
        /// <summary>
        /// A unique identity system ID.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The unique user-chosen identifer associated with the identity. This is used for authentication.
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// The hashed password associated with the identity.
        /// </summary>
        public string HashedPassword { get; set; }

        /// <summary>
        /// The salt used to hash this identity's password.
        /// </summary>
        public byte[] Salt { get; set; }

        /// <summary>
        /// Indicates whether this identity has been disabled, which prevents it from authentication.
        /// </summary>
        public bool Disabled { get; set; }

        /// <summary>
        /// The roles associated with this identity.
        /// </summary>
        public ICollection<Role> Roles { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="Identity"/>.
        /// </summary>
        public Identity()
        {
            Roles = new List<Role>();
        }
    }
}
