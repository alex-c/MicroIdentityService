using MicroIdentityService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroIdentityService.Repositories.InMemory
{
    /// <summary>
    /// A mock implementation of the API key repository, used for testing. This stores API keys in-memory.
    /// </summary>
    public class InMemoryApiKeyRepository : IApiKeyRepository
    {
        private Dictionary<Guid, ApiKey> ApiKeys { get; }

        public InMemoryApiKeyRepository()
        {
            ApiKeys = new Dictionary<Guid, ApiKey>();
        }

        public async Task<IEnumerable<ApiKey>> GetApiKeys()
        {
            return ApiKeys.Values;
        }

        public async Task<IEnumerable<ApiKey>> SearchApiKeysByName(string filter)
        {
            return ApiKeys.Values.Where(a => a.Name.ToLowerInvariant().Contains(filter.ToLowerInvariant()));
        }

        public async Task<ApiKey> GetApiKey(Guid id)
        {
            return ApiKeys.GetValueOrDefault(id);
        }

        public async Task<ApiKey> CreateApiKey(string name)
        {
            ApiKey key = new ApiKey()
            {
                Id = Guid.NewGuid(),
                Name = name,
                Enabled = false
            };

            ApiKeys.Add(key.Id, key);

            return key;
        }

        public async Task<ApiKey> UpdateApiKey(ApiKey apiKey)
        {
            ApiKeys[apiKey.Id] = apiKey;
            return apiKey;
        }

        public async Task DeleteApiKey(Guid id)
        {
            ApiKeys.Remove(id);
        }
    }
}
