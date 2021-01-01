using System;

namespace MicroIdentityService.Models
{
    /// <summary>
    /// An API key which allows access to the MicroIdentityService API.
    /// </summary>
    public class ApiKey
    {
        /// <summary>
        /// The ID of the API key.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Name of the API key.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Whether this key is enabled.
        /// </summary>
        public bool Enabled { get; set; }
    }
}
