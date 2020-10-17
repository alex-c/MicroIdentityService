using System;

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
    }
}
