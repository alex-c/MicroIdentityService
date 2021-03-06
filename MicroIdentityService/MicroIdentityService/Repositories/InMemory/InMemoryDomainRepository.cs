﻿using MicroIdentityService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<IEnumerable<Domain>> GetDomains()
        {
            return SetDomainRoles(DomainIdMap.Values);
        }

        public async Task<IEnumerable<Domain>> SearchDomainsByName(string filter)
        {
            return SetDomainRoles(DomainNameMap.Where(kvp => kvp.Key.ToLowerInvariant().Contains(filter.ToLowerInvariant())).Select(kvp => kvp.Value));
        }

        public async Task<Domain> GetDomain(Guid id)
        {
            if (DomainIdMap.TryGetValue(id, out Domain domain))
            {
                return await SetDomainRoles(domain);
            }
            return null;
        }

        public async Task<Domain> GetDomain(string name)
        {
            if (DomainNameMap.TryGetValue(name, out Domain domain))
            {
                return await SetDomainRoles(domain);
            }
            return null;
        }

        public async Task<Domain> CreateDomain(string name)
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

        public async Task<Domain> UpdateDomain(Domain domain)
        {
            // NoOp: in-memory identity already updated
            return domain;
        }

        public async Task DeleteDomain(Guid id)
        {
            if (DomainIdMap.TryGetValue(id, out Domain domain))
            {
                DomainIdMap.Remove(id);
                DomainNameMap.Remove(domain.Name);
            }
        }

        private async Task<Domain> SetDomainRoles(Domain domain)
        {
            domain.Roles = (await RoleRepository.GetDomainRoles(domain.Id)).ToHashSet();
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
