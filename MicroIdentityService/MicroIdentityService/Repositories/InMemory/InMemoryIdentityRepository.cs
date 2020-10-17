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
        public IEnumerable<Identity> GetIdentities()
        {
            return IdentitiesIdMap.Values;
        }

        private Dictionary<Guid, Identity> IdentitiesIdMap { get; }
        private Dictionary<string, Identity> IdentitiesEmailMap { get; }

        public InMemoryIdentityRepository()
        {
            IdentitiesIdMap = new Dictionary<Guid, Identity>();
            IdentitiesEmailMap = new Dictionary<string, Identity>();
        }

        public Identity GetIdentity(Guid id)
        {
            if (IdentitiesIdMap.TryGetValue(id, out Identity identity))
            {
                return identity;
            }
            return null;
        }

        public Identity GetIdentity(string email)
        {
            if (IdentitiesEmailMap.TryGetValue(email, out Identity identity))
            {
                return identity;
            }
            return null;
        }

        public Identity CreateIdentity(string email, string hashedPassword, byte[] salt)
        {
            Identity identity = new Identity()
            {
                Id = Guid.NewGuid(),
                Email = email,
                HashedPassword = hashedPassword,
                Salt = salt
            };
            IdentitiesIdMap.Add(identity.Id, identity);
            IdentitiesEmailMap.Add(identity.Email, identity);
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
                IdentitiesEmailMap.Remove(identity.Email);
            }
        }
    }
}
