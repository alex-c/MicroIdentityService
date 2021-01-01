using MicroIdentityService.Models;
using MicroIdentityService.Repositories;
using MicroIdentityService.Services.Exceptions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

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
        /// Gets all available API keys, whether they are enabled or not.
        /// </summary>
        /// <returns>Returns all API keys.</returns>
        public IEnumerable<ApiKey> GetAllApiKeys()
        {
            return ApiKeyRepository.GetAllApiKeys();
        }

        /// <summary>
        /// Gets an API key by it's ID.
        /// </summary>
        /// <param name="id">ID of the API key to get.</param>
        /// <returns>Returns the API key if found.</returns>
        /// <exception cref="EntityNotFoundException">Thrown if no matching API key could be found.</exception>
        public ApiKey GetApiKey(Guid id)
        {
            return GetApiKeyOrThrowNotFoundException(id);
        }

        /// <summary>
        /// Creates a new API key. API keys are disabled upon creation.
        /// </summary>
        /// <param name="name">Name of the API key to create.</param>
        /// <returns>Returns the newly created key.</returns>
        public ApiKey CreateApiKey(string name)
        {
            return ApiKeyRepository.CreateApiKey(name);
        }

        /// <summary>
        /// Updates an API key, which allows to change it's name.
        /// </summary>
        /// <param name="id">ID of the key to update.</param>
        /// <param name="name">New name of the key.</param>
        /// <returns>Returns the updated key.</returns>
        /// <exception cref="EntityNotFoundException">Thrown if no matching API key could be found.</exception>
        public ApiKey UpdateApiKey(Guid id, string name)
        {
            ApiKey key = GetApiKeyOrThrowNotFoundException(id);

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
        public void UpdateApiKeyStatus(Guid id, bool isEnabled)
        {
            ApiKey key = GetApiKeyOrThrowNotFoundException(id);

            key.Enabled = isEnabled;
            ApiKeyRepository.UpdateApiKey(key);
        }

        /// <summary>
        /// Deletes an API key.
        /// </summary>
        /// <param name="id">ID of the key to delete.</param>
        public void DeleteApiKey(Guid id)
        {
            ApiKeyRepository.DeleteApiKey(id);
        }

        #region Private Helpers

        /// <summary>
        /// Attempts to get an API key from the underlying repository and throws a <see cref="EntityNotFoundException"/> if no matching key could be found.
        /// </summary>
        /// <param name="id">ID of the key to get.</param>
        /// <exception cref="EntityNotFoundException">Thrown if no matching key could be found.</exception>
        /// <returns>Returns the key, if found.</returns>
        private ApiKey GetApiKeyOrThrowNotFoundException(Guid id)
        {
            ApiKey key = ApiKeyRepository.GetApiKey(id);

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
