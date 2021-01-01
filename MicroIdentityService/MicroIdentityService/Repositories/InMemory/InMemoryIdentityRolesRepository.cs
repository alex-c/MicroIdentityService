using MicroIdentityService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroIdentityService.Repositories.InMemory
{
    public class InMemoryIdentityRolesRepository : IIdentityRoleRepository
    {
        public async Task SetIdentityRoles(Identity identity, IEnumerable<Role> roles)
        {
            identity.Roles = roles;
        }
    }
}
