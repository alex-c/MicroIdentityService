using MicroIdentityService.Models;
using MicroIdentityService.Repositories;
using MicroIdentityService.Services.Exceptions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroIdentityService.Services
{
    /// <summary>
    /// A service for the management of API keys.
    /// </summary>
    public class ApiKeyService
    {
        /// <summary>
        /// The underlying repository containing API keys.
        /// </summary>
        private IApiKeyRepository ApiKeyRepository { get; }

        /// <summary>
        /// A logger for local logging purposes.
        /// </summary>
        private ILogger Logger { get; }

        /// <summary>
        /// Sets up the service with all needed component.
        /// </summary>
        /// <param name="logger">A logger instance for local logging needs.</param>
        /// <param name="apiKeyRepository">Grants access to API keys.</param>
        public ApiKeyService(ILogger<ApiKeyService> logger, IApiKeyRepository apiKeyRepository)
        {
            ApiKeyRepository = apiKeyRepository;
            Logger = logger;
        }

        /// <summary>
        /// Gets available API keys, whether they are enabled or not, optionally filtered by name.
        /// </summary>
        /// <param name="filter">Optional name filter.</param>
        /// <returns>Returns matching API keys.</returns>
        public async Task<IEnumerable<ApiKey>> GetApiKeys(string filter = null)
        {
            if (filter == null)
            {
                return await ApiKeyRepository.GetApiKeys();
            }
            else
            {
                return await ApiKeyRepository.SearchApiKeysByName(filter);
            }
        }

        /// <summary>
        /// Gets an API key by it's ID.
        /// </summary>
        /// <param name="id">ID of the API key to get.</param>
        /// <returns>Returns the API key if found.</returns>
        /// <exception cref="EntityNotFoundException">Thrown if no matching API key could be found.</exception>
        public async Task<ApiKey> GetApiKey(Guid id)
        {
            return await GetApiKeyOrThrowNotFoundException(id);
        }

        /// <summary>
        /// Creates a new API key. API keys are disabled upon creation.
        /// </summary>
        /// <param name="name">Name of the API key to create.</param>
        /// <returns>Returns the newly created key.</returns>
        public async Task<ApiKey> CreateApiKey(string name)
        {
            return await ApiKeyRepository.CreateApiKey(name);
        }

        /// <summary>
        /// Updates an API key, which allows to change it's name.
        /// </summary>
        /// <param name="id">ID of the key to update.</param>
        /// <param name="name">New name of the key.</param>
        /// <returns>Returns the updated key.</returns>
        /// <exception cref="EntityNotFoundException">Thrown if no matching API key could be found.</exception>
        public async Task<ApiKey> UpdateApiKey(Guid id, string name)
        {
            ApiKey key = await GetApiKeyOrThrowNotFoundException(id);

            key.Name = name;
            ApiKeyRepository.UpdateApiKey(key);

            return key;
        }

        /// <summary>
        /// Allows to enable or disable an API key.
        /// </summary>
        /// <param name="id">ID of the key to update the status for.</param>
        /// <param name="isEnabled">Whether the key is supposed to be enabled.</param>
        /// <exception cref="EntityNotFoundException">Thrown if no matching API key could be found.</exception>
        public async Task UpdateApiKeyStatus(Guid id, bool isEnabled)
        {
            ApiKey key = await GetApiKeyOrThrowNotFoundException(id);

            key.Enabled = isEnabled;
            ApiKeyRepository.UpdateApiKey(key);
        }

        /// <summary>
        /// Deletes an API key.
        /// </summary>
        /// <param name="id">ID of the key to delete.</param>
        public async Task DeleteApiKey(Guid id)
        {
            await ApiKeyRepository.DeleteApiKey(id);
        }

        #region Private Helpers

        /// <summary>
        /// Attempts to get an API key from the underlying repository and throws a <see cref="EntityNotFoundException"/> if no matching key could be found.
        /// </summary>
        /// <param name="id">ID of the key to get.</param>
        /// <exception cref="EntityNotFoundException">Thrown if no matching key could be found.</exception>
        /// <returns>Returns the key, if found.</returns>
        private async Task<ApiKey> GetApiKeyOrThrowNotFoundException(Guid id)
        {
            ApiKey key = await ApiKeyRepository.GetApiKey(id);

            // Check for key existence
            if (key == null)
            {
                throw new EntityNotFoundException("ApiKey", id);
            }

            return key;
        }

        #endregion
    }
}
