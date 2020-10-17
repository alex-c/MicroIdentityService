using MicroIdentityService.Models;
using System;
using System.Collections.Generic;

namespace MicroIdentityService.Repositories.InMemory
{
    /// <summary>
    /// A mock implementation of the identity repository, used for testing. This stores identities in-memory.
    /// </summary>
    public class InMemoryIdentityRepository : IIdentityRepository, IReadOnlyIdentityRepository
    {
        private Dictionary<Guid, Identity> IdentitiesIdMap { get; }
        private Dictionary<string, Identity> IdentitiesIdentifierMap { get; }

        public InMemoryIdentityRepository()
        {
            IdentitiesIdMap = new Dictionary<Guid, Identity>();
            IdentitiesIdentifierMap = new Dictionary<string, Identity>();
        }

        public IEnumerable<Identity> GetIdentities()
        {
            return IdentitiesIdMap.Values;
        }

        public Identity GetIdentity(Guid id)
        {
            if (IdentitiesIdMap.TryGetValue(id, out Identity identity))
            {
                return identity;
            }
            return null;
        }

        public Identity GetIdentity(string identifier)
        {
            if (IdentitiesIdentifierMap.TryGetValue(identifier, out Identity identity))
            {
                return identity;
            }
            return null;
        }

        public Identity CreateIdentity(string identifier, string hashedPassword, byte[] salt)
        {
            Identity identity = new Identity()
            {
                Id = Guid.NewGuid(),
                Identifier = identifier,
                HashedPassword = hashedPassword,
                Salt = salt
            };
            IdentitiesIdMap.Add(identity.Id, identity);
            IdentitiesIdentifierMap.Add(identity.Identifier, identity);
            return identity;
        }

        public Identity UpdateIdentity(Identity identity)
        {
            // NoOp: in-memory identity already updated
            return identity;
        }

        public void DeleteIdentity(Guid id)
        {
            if (IdentitiesIdMap.TryGetValue(id, out Identity identity))
            {
                IdentitiesIdMap.Remove(id);
                IdentitiesIdentifierMap.Remove(identity.Identifier);
            }
        }
    }
}
