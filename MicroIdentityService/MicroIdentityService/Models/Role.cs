using System;

namespace MicroIdentityService.Models
{
    /// <summary>
    /// An identity role for MIS to manage.
    /// </summary>
    public class Role
    {
        /// <summary>
        /// Unique ID of the role.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Unique name of the role.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The optional domain that this roles belongs to.
        /// </summary>
        public Guid? DomainId { get; set; }
    }
}
