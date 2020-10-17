using MicroIdentityService.Models;
using System;
using System.Collections.Generic;

namespace MicroIdentityService.Repositories.InMemory
{
    /// <summary>
    /// A mock implementation of the domain repository, used for testing. This stores domains in-memory.
    /// </summary>
    public class InMemoryDomainRepository : IDomainRepository
    {
        private Dictionary<Guid, Domain> DomainIdMap { get; }
        private Dictionary<string, Domain> DomainNameMap { get; }

        public InMemoryDomainRepository()
        {
            DomainIdMap = new Dictionary<Guid, Domain>();
            DomainNameMap = new Dictionary<string, Domain>();
        }

        public IEnumerable<Domain> GetDomains()
        {
            return DomainIdMap.Values;
        }

        public Domain GetDomain(Guid id)
        {
            if (DomainIdMap.TryGetValue(id, out Domain domain))
            {
                return domain;
            }
            return null;
        }

        public Domain GetDomain(string name)
        {
            if (DomainNameMap.TryGetValue(name, out Domain domain))
            {
                return domain;
            }
            return null;
        }

        public Domain CreateDomain(string name)
        {
            Domain domain = new Domain()
            {
                Id = Guid.NewGuid(),
                Name = name
            };
            DomainIdMap.Add(domain.Id, domain);
            DomainNameMap.Add(domain.Name, domain);
            return domain;
        }

        public Domain UpdateDomain(Domain domain)
        {
            // NoOp: in-memory identity already updated
            return domain;
        }

        public void DeleteDomain(Guid id)
        {
            if (DomainIdMap.TryGetValue(id, out Domain domain))
            {
                DomainIdMap.Remove(id);
                DomainNameMap.Remove(domain.Name);
            }
        }
    }
}
