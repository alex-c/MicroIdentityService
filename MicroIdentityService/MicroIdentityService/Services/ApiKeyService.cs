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
        /// The underlying repository pf API key permissionss.
        /// </summary>
        private IApiKeyPermissionRepository ApiKeyPermissionRepository { get; }

        /// <summary>
        /// Grants access to existing permission names.
        /// </summary>
        private PermissionService PermissionService { get; }

        /// <summary>
        /// A logger for local logging purposes.
        /// </summary>
        private ILogger Logger { get; }

        /// <summary>
        /// Sets up the service with all needed component.
        /// </summary>
        /// <param name="logger">A logger instance for local logging needs.</param>
        /// <param name="apiKeyRepository">Grants access to API keys.</param>
        /// <param name="apiKeyPermissionRepository">Grants write access to API key permissionss.</param>
        /// <param name="permissionService">Grants access to existing permission names.</param>
        public ApiKeyService(ILogger<ApiKeyService> logger,
            IApiKeyRepository apiKeyRepository,
            IApiKeyPermissionRepository apiKeyPermissionRepository,
            PermissionService permissionService)
        {
            ApiKeyRepository = apiKeyRepository;
            ApiKeyPermissionRepository = apiKeyPermissionRepository;
            PermissionService = permissionService;
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
            ApiKey key = await ApiKeyRepository.GetApiKey(id);
            if (key == null)
            {
                throw new EntityNotFoundException("ApiKey", id);
            }
            return key;
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
            ApiKey key = await GetApiKey(id);

            key.Name = name;
            await ApiKeyRepository.UpdateApiKey(key);

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
            ApiKey key = await GetApiKey(id);

            key.Enabled = isEnabled;
            await ApiKeyRepository.UpdateApiKey(key);
        }

        /// <summary>
        /// Deletes an API key.
        /// </summary>
        /// <param name="id">ID of the key to delete.</param>
        public async Task DeleteApiKey(Guid id)
        {
            await ApiKeyRepository.DeleteApiKey(id);
        }

        /// <summary>
        /// Gets permissions assigned to a given API key.
        /// </summary>
        /// <param name="id">ID of the API key to get permissions for.</param>
        /// <returns>Returns a list of API key permissions.</returns>
        /// <exception cref="EntityNotFoundException">Thrown if the API key could not be found.</exception>
        public async Task<IEnumerable<string>> GetApiKeyPermissions(Guid id)
        {
            ApiKey apiKey = await GetApiKey(id);
            return apiKey.Permissions;
        }

        /// <summary>
        /// Sets the permissions assigned to a given API key.
        /// </summary>
        /// <param name="id">ID of the API key for which to update permissions.</param>
        /// <param name="roleIds">The IDs of the permissions to set.</param>
        /// <exception cref="EntityNotFoundException">Thrown if the API key or any of the permissions could not be found.</exception>
        public async Task UpdateApiKeyPermissions(Guid id, IEnumerable<string> permissions)
        {
            ApiKey apiKey = await GetApiKey(id);

            // Validate permissions existence
            PermissionService.ValidatePermissions(permissions);

            await ApiKeyPermissionRepository.SetApiKeyPermissions(apiKey, permissions);
        }
    }
}
