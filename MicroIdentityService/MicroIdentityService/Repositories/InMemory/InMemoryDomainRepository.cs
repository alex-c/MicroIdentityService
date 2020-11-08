using MicroIdentityService.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MicroIdentityService.Repositories.InMemory
{
    /// <summary>
    /// A mock implementation of the domain repository, used for testing. This stores domains in-memory.
    /// </summary>
    public class InMemoryDomainRepository : IDomainRepository
    {
        private Dictionary<Guid, Domain> DomainIdMap { get; }
        private Dictionary<string, Domain> DomainNameMap { get; }

        private IRoleRepository RoleRepository { get; }

        public InMemoryDomainRepository(IRoleRepository roleRepository)
        {
            DomainIdMap = new Dictionary<Guid, Domain>();
            DomainNameMap = new Dictionary<string, Domain>();
            RoleRepository = roleRepository;
        }

        public IEnumerable<Domain> GetDomains()
        {
            return SetDomainRoles(DomainIdMap.Values);
        }

        public IEnumerable<Domain> SearchDomainsByName(string filter)
        {
            return SetDomainRoles(DomainNameMap.Where(kvp => kvp.Key.ToLowerInvariant().Contains(filter.ToLowerInvariant())).Select(kvp => kvp.Value));
        }

        public Domain GetDomain(Guid id)
        {
            if (DomainIdMap.TryGetValue(id, out Domain domain))
            {
                return SetDomainRoles(domain);
            }
            return null;
        }

        public Domain GetDomain(string name)
        {
            if (DomainNameMap.TryGetValue(name, out Domain domain))
            {
                return SetDomainRoles(domain);
            }
            return null;
        }

        public Domain CreateDomain(string name)
        {
            Domain domain = new Domain()
            {
                Id = Guid.NewGuid(),
                Name = name,
                Roles = new HashSet<Role>()
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

        private Domain SetDomainRoles(Domain domain)
        {
            domain.Roles = RoleRepository.GetDomainRoles(domain.Id).ToHashSet();
            return domain;
        }

        private IEnumerable<Domain> SetDomainRoles(IEnumerable<Domain> domains)
        {
            foreach (Domain domain in domains)
            {
                SetDomainRoles(domain);
            }
            return domains;
        }
    }
}
