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
    public class SqlRoleRepository : BaseSqlRepository, IRoleRepository
    {
        public SqlRoleRepository(IConfiguration configuration, ILogger<SqlRoleRepository> logger) : base(configuration, logger) { }

        public async Task<IEnumerable<Role>> GetRoles()
        {
            string sql = "SELECT * FROM roles;";
            return await QueryMultipleRoles(sql);
        }

        public async Task<IEnumerable<Role>> GetRoles(IEnumerable<Guid> ids)
        {
            string sql = "SELECT * FROM roles WHERE id IN @Ids;";
            return await QueryMultipleRoles(sql, new { ids });
        }

        public async Task<IEnumerable<Role>> GetDomainRoles(Guid domainId)
        {
            string sql = "SELECT * FROM roles WHERE domain_id=@DomainId;";
            return await QueryMultipleRoles(sql, new { domainId });
        }

        public async Task<IEnumerable<Role>> SearchRolesByName(string filter)
        {
            string sql = "SELECT * FROM roles WHERE name LIKE @Filter;";
            return await QueryMultipleRoles(sql, new { Filter = $"&{filter}&" });
        }

        public async Task<IEnumerable<Role>> SearchDomainRolesByName(Guid domainId, string filter)
        {
            string sql = "SELECT * FROM roles WHERE domain_id=@DomainId AND name LIKE @Filter";
            return await QueryMultipleRoles(sql, new { domainId, Filter = $"&{filter}&" });
        }

        public async Task<Role> GetRole(Guid id)
        {
            string sql = "SELECT * FROM roles WHERE id=@Id";
            return await QuerySingleRole(sql, new { id });
        }

        public async Task<Role> GetRole(string name, Guid? domainId)
        {
            string sql = "SELECT * FROM roles WHERE name=@Name";
            if (domainId.HasValue)
            {
                sql += " AND domain_id=@DomainId";
            }
            return await QuerySingleRole(sql, new { name, domainId });
        }

        public async Task<Role> CreateRole(string name, Guid? domainId)
        {
            Guid id = Guid.NewGuid();

            Role role = null;
            using (IDbConnection connection = GetNewConnection())
            {
                await connection.ExecuteAsync("INSERT INTO roles (id, name, domain_id) VALUES (@Id, @Name, @DomainId)", new { id, name, domainId });
                role = await QuerySingleRole(connection, id);
            }
            return role;
        }

        public async Task<Role> UpdateRole(Role role)
        {
            using (IDbConnection connection = GetNewConnection())
            {
                await connection.ExecuteAsync("UPDATE roles SET name=@Name, domain_id=@DomainId WHERE id=@Id", role);
            }
            return role;
        }

        public async Task DeleteRole(Guid id)
        {
            using (IDbConnection connection = GetNewConnection())
            {
                await connection.ExecuteAsync("DELETE FROM roles WHERE id=@Id", new { id });
            }
        }

        private async Task<IEnumerable<Role>> QueryMultipleRoles(string sql, object parameters = null)
        {
            IEnumerable<Role> roles = null;
            using (IDbConnection connection = GetNewConnection())
            {
                roles = await connection.QueryAsync<Role>(sql, parameters);
            }
            return roles;
        }

        private async Task<Role> QuerySingleRole(string sql, object parameters = null)
        {
            Role role = null;
            using (IDbConnection connection = GetNewConnection())
            {
                role = await QuerySingleRole(connection, sql, parameters);
            }
            return role;
        }

        private async Task<Role> QuerySingleRole(IDbConnection connection, Guid id)
        {
            string sql = "SELECT * FROM roles WHERE id=@Id";
            return await QuerySingleRole(connection, sql, new { id });
        }

        private async Task<Role> QuerySingleRole(IDbConnection connection, string sql, object parameters = null)
        {
            return await connection.QueryFirstOrDefaultAsync<Role>(sql, parameters);
        }
    }
}
