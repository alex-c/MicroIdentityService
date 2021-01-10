using MicroIdentityService.Models;
using System;
using System.Collections.Generic;

namespace MicroIdentityService.Controllers.Contracts.Responses
{
    /// <summary>
    /// Response contract for API keys.
    /// </summary>
    public class ApiKeyResponse
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

        /// <summary>
        /// Permissions granted by this API key.
        /// </summary>
        public IEnumerable<string> Permissions { get; set; }

        /// <summary>
        /// Generates an API key response contract from an API key model.
        /// </summary>
        /// <param name="key">Key to generate an ID for.</param>
        public ApiKeyResponse(ApiKey key)
        {
            Id = key.Id;
            Name = key.Name;
            Enabled = key.Enabled;
            Permissions = key.Permissions;
        }
    }
}
