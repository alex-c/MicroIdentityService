using MicroIdentityService.Models;
using System;
using System.Collections.Generic;

namespace MicroIdentityService.Repositories.InMemory
{
    public class InMemoryIdentityRolesRepository : IIdentityRoleRepository
    {
        public void SetIdentityRoles(Identity identity, IEnumerable<Role> roles)
        {
            identity.Roles = roles;
        }
    }
}
