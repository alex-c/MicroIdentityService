using Dapper;
using MicroIdentityService.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MicroIdentityService.Repositories.Sql
{
    public class SqlApiKeyRepository : BaseSqlRepository, IApiKeyRepository
    {
        public SqlApiKeyRepository(IConfiguration configuration, ILogger<SqlApiKeyRepository> logger) : base(configuration, logger) { }

        public async Task<IEnumerable<ApiKey>> GetApiKeys()
        {
            string sql = "SELECT * FROM api_keys k LEFT JOIN api_key_permissions p ON p.api_key_id=k.id";
            return await QueryMultipleApiKeys(sql);
        }

        public async Task<IEnumerable<ApiKey>> SearchApiKeysByName(string filter)
        {
            string sql = "SELECT * FROM api_keys k LEFT JOIN api_key_permissions p ON p.api_key_id=k.id WHERE name ILIKE @Filter";
            return await QueryMultipleApiKeys(sql, new { Filter = $"%{filter}%" });
        }

        public async Task<ApiKey> GetApiKey(Guid id)
        {
            ApiKey apiKey = null;
            using (IDbConnection connection = GetNewConnection())
            {
                apiKey = await QuerySingleApiKey(connection, id);
            }
            return apiKey;
        }

        public async Task<ApiKey> CreateApiKey(string name)
        {
            Guid id = Guid.NewGuid();

            ApiKey key = null;
            using (IDbConnection connection = GetNewConnection())
            {
                await connection.ExecuteAsync("INSERT INTO api_keys (id, name) VALUES (@Id, @Name)", new { id, name });
                key = await QuerySingleApiKey(connection, id);
            }
            return key;
        }

        public async Task<ApiKey> UpdateApiKey(ApiKey apiKey)
        {
            using (IDbConnection connection = GetNewConnection())
            {
                await connection.ExecuteAsync("UPDATE api_keys SET name=@Name, enabled=@Enabled WHERE id=@Id", new { apiKey.Id, apiKey.Name, apiKey.Enabled });
            }
            return apiKey;
        }

        public async Task DeleteApiKey(Guid id)
        {
            using (IDbConnection connection = GetNewConnection())
            {
                await connection.ExecuteAsync("DELETE FROM api_key_permissions WHERE api_key_id=@Id", new { id });
                await connection.ExecuteAsync("DELETE FROM api_keys WHERE id=@Id", new { id });
            }
        }

        public async Task<IEnumerable<ApiKey>> QueryMultipleApiKeys(string sql, object parameters = null)
        {
            IEnumerable<ApiKey> apiKeys = null;
            Dictionary<Guid, ApiKey> apiKeyMap = new Dictionary<Guid, ApiKey>();
            using (IDbConnection connection = GetNewConnection())
            {
                apiKeys = await connection.QueryAsync<ApiKey, PermissionRow, ApiKey>(sql,
                    (queryApiKey, permissionRow) =>
                    {
                        ApiKey apiKey = null;
                        if (!apiKeyMap.TryGetValue(queryApiKey.Id, out apiKey))
                        {
                            apiKey = queryApiKey;
                            apiKeyMap.Add(apiKey.Id, apiKey);
                        }
                        if (permissionRow != null)
                        {
                            apiKey.Permissions.Add(permissionRow.Permission);
                        }
                        return apiKey;
                    },
                    param: parameters,
                    splitOn: "api_key_id");
            }
            return apiKeys.Distinct();
        }

        private async Task<ApiKey> QuerySingleApiKey(IDbConnection connection, Guid id)
        {
            string sql = "SELECT * FROM api_keys k LEFT JOIN api_key_permissions p ON p.api_key_id=k.id WHERE k.id=@Id";
            IEnumerable<ApiKey> apiKeys = null;
            Dictionary<Guid, ApiKey> apiKeyMap = new Dictionary<Guid, ApiKey>();
            apiKeys = await connection.QueryAsync<ApiKey, PermissionRow, ApiKey>(sql,
                (queryApiKey, permissionRow) =>
                {
                    ApiKey apiKey = null;
                    if (!apiKeyMap.TryGetValue(queryApiKey.Id, out apiKey))
                    {
                        apiKey = queryApiKey;
                        apiKeyMap.Add(apiKey.Id, apiKey);
                    }
                    if (permissionRow != null)
                    {
                        apiKey.Permissions.Add(permissionRow.Permission);
                    }
                    return apiKey;
                },
                param: new { id },
                splitOn: "api_key_id");
            return apiKeys.Distinct().FirstOrDefault();
        }

        /// <summary>
        /// Represents an API key permission as saved in the database.
        /// </summary>
        internal class PermissionRow
        {
            /// <summary>
            /// The ID of the API key this permission is assigned to.
            /// </summary>
            public Guid ApiKeyId { get; set; }

            /// <summary>
            /// The actual permission name.
            /// </summary>
            public string Permission { get; set; }
        }
    }
}
