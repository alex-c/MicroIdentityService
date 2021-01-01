using Dapper;
using MicroIdentityService.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace MicroIdentityService.Repositories.Sql
{
    public class SqlApiKeyRepository : BaseSqlRepository, IApiKeyRepository
    {
        public SqlApiKeyRepository(IConfiguration configuration, ILogger<SqlApiKeyRepository> logger) : base(configuration, logger) { }

        public async Task<IEnumerable<ApiKey>> GetApiKeys()
        {
            string sql = "SELECT * FROM api_keys";

            IEnumerable<ApiKey> apiKeys = null;
            using (IDbConnection connection = GetNewConnection())
            {
                apiKeys = await connection.QueryAsync<ApiKey>(sql);
            }
            return apiKeys;
        }

        public async Task<IEnumerable<ApiKey>> SearchApiKeysByName(string filter)
        {
            string sql = "SELECT * FROM api_keys WHERE name ILIKE @Filter";

            IEnumerable<ApiKey> apiKeys = null;
            using (IDbConnection connection = GetNewConnection())
            {
                apiKeys = await connection.QueryAsync<ApiKey>(sql, new { Filter = $"%{filter}%" });
            }
            return apiKeys;
        }

        public async Task<ApiKey> GetApiKey(Guid id)
        {
            ApiKey apiKey = null;
            using (IDbConnection connection = GetNewConnection())
            {
                apiKey = await GetApiKey(connection, id);
            }
            return apiKey;
        }

        public async Task<ApiKey> GetApiKey(IDbConnection connection, Guid id)
        {
            string sql = "SELECT * FROM api_keys WHERE id=@Id";
            return await connection.QueryFirstOrDefaultAsync<ApiKey>(sql, new { id });
        }

        public async Task<ApiKey> CreateApiKey(string name)
        {
            Guid id = Guid.NewGuid();

            ApiKey key = null;
            using (IDbConnection connection = GetNewConnection())
            {
                await connection.ExecuteAsync("INSERT INTO api_keys (id, name) VALUES (@Id, @Name)", new
                {
                    Id = id,
                    Name = name
                });
                key = await GetApiKey(connection, id);
            }
            return key;
        }

        public async Task UpdateApiKey(ApiKey apiKey)
        {
            using (IDbConnection connection = GetNewConnection())
            {
                await connection.ExecuteAsync("UPDATE api_keys SET name=@Name, enabled=@Enabled WHERE id=@Id", new { apiKey.Id, apiKey.Name, apiKey.Enabled });
            }
        }

        public async Task DeleteApiKey(Guid id)
        {
            using (IDbConnection connection = GetNewConnection())
            {
                await connection.ExecuteAsync("DELETE FROM api_keys WHERE id=@Id", new { id });
            }
        }
    }
}
