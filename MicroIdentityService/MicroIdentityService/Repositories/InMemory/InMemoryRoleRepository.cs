using MicroIdentityService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroIdentityService.Repositories.InMemory
{
    /// <summary>
    /// A mock implementation of the role repository, used for testing. This stores roles in-memory.
    /// </summary>
    public class InMemoryRoleRepository : IRoleRepository
    {
        private Dictionary<Guid, Role> Roles { get; }

        public InMemoryRoleRepository()
        {
            Roles = new Dictionary<Guid, Role>();
        }

        public async Task<IEnumerable<Role>> GetRoles()
        {
            return Roles.Values;
        }

        public async Task<IEnumerable<Role>> GetRoles(IEnumerable<Guid> ids)
        {
            return Roles.Values.Where(r => ids.Contains(r.Id));
        }

        public async Task<IEnumerable<Role>> SearchRolesByName(string filter)
        {
            return (await GetRoles()).Where(r => r.Name.ToLowerInvariant().Contains(filter.ToLowerInvariant()));
        }

        public async Task<IEnumerable<Role>> GetDomainRoles(Guid domainId)
        {
            return (await GetRoles()).Where(r => r.DomainId == domainId);
        }

        public async Task<IEnumerable<Role>> SearchDomainRolesByName(Guid domainId, string filter)
        {
            return (await GetDomainRoles(domainId)).Where(r => r.Name.ToLowerInvariant().Contains(filter.ToLowerInvariant()));
        }

        public async Task<Role> GetRole(Guid id)
        {
            if (Roles.TryGetValue(id, out Role role))
            {
                return role;
            }
            return null;
        }

        public async Task<Role> GetRole(string name, Guid? domainId)
        {
            return Roles.Values.FirstOrDefault(r => r.Name == name && r.DomainId == domainId);
        }

        public async Task<Role> CreateRole(string name, Guid? domainId)
        {
            Role role = new Role()
            {
                Id = Guid.NewGuid(),
                Name = name,
                DomainId = domainId
            };
            Roles.Add(role.Id, role);
            return role;
        }

        public async Task<Role> UpdateRole(Role role)
        {
            // NoOp: in-memory identity already updated
            return role;
        }

        public async Task DeleteRole(Guid id)
        {
            Roles.Remove(id);
        }
    }
}
