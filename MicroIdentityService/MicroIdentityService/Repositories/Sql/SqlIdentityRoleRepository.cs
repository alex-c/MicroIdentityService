using Dapper;
using MicroIdentityService.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace MicroIdentityService.Repositories.Sql
{
    public class SqlIdentityRoleRepository : BaseSqlRepository, IIdentityRoleRepository
    {
        public SqlIdentityRoleRepository(IConfiguration configuration, ILogger<SqlIdentityRoleRepository> logger) : base(configuration, logger) { }

        public async Task SetIdentityRoles(Identity identity, IEnumerable<Role> roles)
        {
            using (IDbConnection connection = GetNewConnection())
            {
                await connection.ExecuteAsync("DELETE FROM identity_roles WHERE identity_id=@id", new { identity.Id });
                foreach (Role role in roles)
                {
                    await connection.ExecuteAsync("INSERT INTO identity_roles (identity_id, role_id) VALUES (@IdentityId, @RoleId)",
                        new
                        {
                            IdentityId = identity.Id,
                            RoleId = role.Id
                        });
                }
            }
        }
    }
}
