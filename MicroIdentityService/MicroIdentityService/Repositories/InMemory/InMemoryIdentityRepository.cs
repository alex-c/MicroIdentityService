using MicroIdentityService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroIdentityService.Repositories.InMemory
{
    /// <summary>
    /// A mock implementation of the identity repository, used for testing. This stores identities in-memory.
    /// </summary>
    public class InMemoryIdentityRepository : IIdentityRepository
    {
        private Dictionary<Guid, Identity> IdentitiesIdMap { get; }
        private Dictionary<string, Identity> IdentitiesIdentifierMap { get; }

        public InMemoryIdentityRepository()
        {
            IdentitiesIdMap = new Dictionary<Guid, Identity>();
            IdentitiesIdentifierMap = new Dictionary<string, Identity>();
        }

        public async Task<IEnumerable<Identity>> GetIdentities(bool showDisabled)
        {
            if (!showDisabled)
            {
                return IdentitiesIdMap.Values.Where(i => i.Disabled == false);
            }
            return IdentitiesIdMap.Values;
        }

        public async Task<IEnumerable<Identity>> SearchIdentitiesByIdentifier(string filter, bool showDisabled)
        {
            if (!showDisabled)
            {
                return IdentitiesIdentifierMap.Where(kvp => kvp.Key.ToLowerInvariant().Contains(filter.ToLowerInvariant())).Select(kvp => kvp.Value).Where(i => i.Disabled == false);
            }
            return IdentitiesIdentifierMap.Where(kvp => kvp.Key.ToLowerInvariant().Contains(filter.ToLowerInvariant())).Select(kvp => kvp.Value);
        }

        public async Task<Identity> GetIdentity(Guid id)
        {
            if (IdentitiesIdMap.TryGetValue(id, out Identity identity))
            {
                return identity;
            }
            return null;
        }

        public async Task<Identity> GetIdentity(string identifier)
        {
            if (IdentitiesIdentifierMap.TryGetValue(identifier, out Identity identity))
            {
                return identity;
            }
            return null;
        }

        public async Task<Identity> CreateIdentity(string identifier, string hashedPassword, byte[] salt)
        {
            Identity identity = new Identity()
            {
                Id = Guid.NewGuid(),
                Identifier = identifier,
                HashedPassword = hashedPassword,
                Salt = salt,
                Disabled = false,
                Roles = new List<Role>()
            };
            IdentitiesIdMap.Add(identity.Id, identity);
            IdentitiesIdentifierMap.Add(identity.Identifier, identity);
            return identity;
        }

        public async Task<Identity> UpdateIdentity(Identity identity)
        {
            // NoOp: in-memory identity already updated
            return identity;
        }

        public async Task DeleteIdentity(Guid id)
        {
            if (IdentitiesIdMap.TryGetValue(id, out Identity identity))
            {
                IdentitiesIdMap.Remove(id);
                IdentitiesIdentifierMap.Remove(identity.Identifier);
            }
        }
    }
}
