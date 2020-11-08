using System;
using System.Collections.Generic;

namespace MicroIdentityService.Models
{
    /// <summary>
    /// A domain for which MIS can manage roles.
    /// </summary>
    public class Domain
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
        /// Roles associated with this domain.
        /// </summary>
        public ISet<Role> Roles { get; set; }
    }
}
