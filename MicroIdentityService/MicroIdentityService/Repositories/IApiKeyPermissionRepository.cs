using MicroIdentityService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroIdentityService.Repositories
{
    /// <summary>
    /// A repository for API key permission mappings.
    /// </summary>
    public interface IApiKeyPermissionRepository
    {
        /// <summary>
        /// Sets API key permissions. This removes previous API key permissions and sets the new ones.
        /// </summary>
        /// <param name="apiKey">API key for which to set permissions.</param>
        /// <param name="roles">Permissions to set.</param>
        Task SetApiKeyPermissions(ApiKey apiKey, IEnumerable<string> permissions);
    }
}
