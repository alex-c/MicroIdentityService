using MicroIdentityService.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroIdentityService.Repositories
{
    /// <summary>
    /// Generic interface for a repository of API keys.
    /// </summary>
    public interface IApiKeyRepository
    {
        /// <summary>
        /// Gets all available API keys.
        /// </summary>
        /// <returns>Returns all API keys.</returns>
        Task<IEnumerable<ApiKey>> GetApiKeys();

        /// <summary>
        /// Searches API keys by name. All API keys containing the search string in the name (case insensitive) are returned.
        /// </summary>
        /// <param name="filter">A string to filter API key names with.</param>
        /// <returns>Returns all matching API keys.</returns>
        Task<IEnumerable<ApiKey>> SearchApiKeysByName(string filter);

        /// <summary>
        /// Gets an API key by it's ID.
        /// </summary>
        /// <param name="id">ID of the key to get.</param>
        /// <returns>Returns the API key if found, else null.</returns>
        Task<ApiKey> GetApiKey(Guid id);

        /// <summary>
        /// Creates a new API key.
        /// </summary>
        /// <param name="name">Name of the API key to create.</param>
        /// <returns>Returns the newly crated API key.</returns>
        Task<ApiKey> CreateApiKey(string name);

        /// <summary>
        /// Updates an existing API key.
        /// </summary>
        /// <param name="apiKey">key to update.</param>
        Task UpdateApiKey(ApiKey apiKey);

        /// <summary>
        /// Deletes an API key.
        /// </summary>
        /// <param name="id">ID of the key to delete.</param>
        Task DeleteApiKey(Guid id);
    }
}
