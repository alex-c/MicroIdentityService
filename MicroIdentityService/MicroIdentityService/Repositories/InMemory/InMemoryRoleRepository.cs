using MicroIdentityService.Models;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public IEnumerable<Role> GetRoles()
        {
            return Roles.Values;
        }

        public IEnumerable<Role> GetDomainRoles(Guid domainId)
        {
            return Roles.Values.Where(r => r.DomainId == domainId);
        }

        public Role GetRole(Guid id)
        {
            if (Roles.TryGetValue(id, out Role role))
            {
                return role;
            }
            return null;
        }

        public Role GetRole(string name, Guid? domainId)
        {
            return Roles.Values.FirstOrDefault(r => r.Name == name && r.DomainId == domainId);
        }

        public Role CreateRole(string name, Guid? domainId)
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

        public Role UpdateRole(Role role)
        {
            // NoOp: in-memory identity already updated
            return role;
        }

        public void DeleteRole(Guid id)
        {
            Roles.Remove(id);
        }
    }
}
