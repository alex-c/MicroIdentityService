using Dapper;
using MicroIdentityService.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace MicroIdentityService.Repositories.Sql
{
    public class SqlApiKeyPermissionRepository : BaseSqlRepository, IApiKeyPermissionRepository
    {
        public SqlApiKeyPermissionRepository(IConfiguration configuration, ILogger<SqlApiKeyPermissionRepository> logger) : base(configuration, logger) { }

        public async Task SetApiKeyPermissions(ApiKey apiKey, IEnumerable<string> permissions)
        {
            using (IDbConnection connection = GetNewConnection())
            {
                await connection.ExecuteAsync("DELETE FROM api_key_permissions WHERE api_key_id=@Id", apiKey);
                foreach (string permission in permissions)
                {
                    await connection.ExecuteAsync("INSERT INTO api_key_permissions (api_key_id, permission) VALUES (@ApiKeyId, @Permission)",
                        new
                        {
                            ApiKeyId = apiKey.Id,
                            permission
                        });
                }
            }
        }
    }
}
