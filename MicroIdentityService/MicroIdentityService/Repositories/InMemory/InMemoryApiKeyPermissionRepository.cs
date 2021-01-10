using MicroIdentityService.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroIdentityService.Repositories.InMemory
{
    public class InMemoryApiKeyPermissionRepository : IApiKeyPermissionRepository
    {
        public async Task SetApiKeyPermissions(ApiKey apiKey, IEnumerable<string> permissions)
        {
            apiKey.Permissions = permissions.ToList();
        }
    }
}
