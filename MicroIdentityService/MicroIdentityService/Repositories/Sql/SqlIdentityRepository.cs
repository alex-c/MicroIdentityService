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
    public class SqlIdentityRepository : BaseSqlRepository, IIdentityRepository
    {
        public SqlIdentityRepository(IConfiguration configuration, ILogger<SqlIdentityRepository> logger) : base(configuration, logger) { }

        public async Task<IEnumerable<Identity>> GetIdentities()
        {
            string sql = "SELECT * FROM identities i LEFT JOIN identity_roles ir ON ir.identity_id=i.id LEFT JOIN roles r ON r.id=ir.role_id";
            return await QueryMultipleIdentities(sql);
        }

        public async Task<IEnumerable<Identity>> SearchIdentitiesByIdentifier(string filter)
        {
            string sql = "SELECT * FROM identities i LEFT JOIN identity_roles ir ON ir.identity_id=i.id LEFT JOIN roles r ON r.id=ir.role_id WHERE i.name LIKE @Filter";
            return await QueryMultipleIdentities(sql, new { Filter = $"%{filter}%" });
        }

        public async Task<Identity> GetIdentity(Guid id)
        {
            Identity identity = null;
            using (IDbConnection connection = GetNewConnection())
            {
                identity = await QuerySingleIdentity(connection, id);
            }
            return identity;
        }

        public async Task<Identity> GetIdentity(string identifier)
        {
            string sql = "SELECT * FROM identities i LEFT JOIN identity_roles ir ON ir.identity_id=i.id LEFT JOIN roles r ON r.id=ir.role_id WHERE i.identifier=@Identifier";
            Identity identity = null;
            using (IDbConnection connection = GetNewConnection())
            {
                identity = await QuerySingleIdentity(connection, sql, new { identifier });
            }
            return identity;
        }

        public async Task<Identity> CreateIdentity(string identifier, string hashedPassword, byte[] salt)
        {
            Guid id = Guid.NewGuid();

            Identity identity = null;
            using (IDbConnection connection = GetNewConnection())
            {
                await connection.ExecuteAsync("INSERT INTO identities (id, identifier, password, salt) VALUES (@Id, @Identifier, @Password, @Salt)",
                    new
                    {
                        id,
                        identifier,
                        Password = hashedPassword,
                        salt
                    });
                identity = await QuerySingleIdentity(connection, id);
            }
            return identity;
        }

        public async Task<Identity> UpdateIdentity(Identity identity)
        {
            using (IDbConnection connection = GetNewConnection())
            {
                await connection.ExecuteAsync("UPDATE identities SET identifier=@Identifier, password=@HashedPassword, salt=@Salt, disabled=@Disabled WHERE id=@Id", identity);
            }
            return identity;
        }

        public async Task DeleteIdentity(Guid id)
        {
            using (IDbConnection connection = GetNewConnection())
            {
                await connection.ExecuteAsync("DELETE FROM identities WHERE id=@Id", new { id });
            }
        }

        private async Task<IEnumerable<Identity>> QueryMultipleIdentities(string sql, object parameters = null)
        {
            IEnumerable<Identity> identities = null;
            Dictionary<Guid, Identity> identityMap = new Dictionary<Guid, Identity>();
            using (IDbConnection connection = GetNewConnection())
            {
                identities = await connection.QueryAsync<Identity, Role, Identity>(sql,
                    (queryIdentity, role) =>
                    {
                        Identity identity = null;
                        if (!identityMap.TryGetValue(queryIdentity.Id, out identity))
                        {
                            identity = queryIdentity;
                            identityMap.Add(identity.Id, identity);
                        }
                        if (role != null)
                        {
                            identity.Roles.Add(role);
                        }
                        return identity;
                    },
                    param: parameters,
                    splitOn: "id");
            }
            return identities.Distinct();
        }

        private async Task<Identity> QuerySingleIdentity(IDbConnection connection, Guid id)
        {
            string sql = "SELECT * FROM identities i LEFT JOIN identity_roles ir ON ir.identity_id=i.id LEFT JOIN roles r ON r.id=ir.role_id WHERE i.id=@Id";
            return await QuerySingleIdentity(connection, sql, new { id });
        }

        private async Task<Identity> QuerySingleIdentity(IDbConnection connection, string sql, object parameters, string splitOn = "id")
        {
            IEnumerable<Identity> identities = null;
            Dictionary<Guid, Identity> identityMap = new Dictionary<Guid, Identity>();
            identities = await connection.QueryAsync<Identity, Role, Identity>(sql,
                (queryIdentity, role) =>
                {
                    Identity identity = null;
                    if (!identityMap.TryGetValue(queryIdentity.Id, out identity))
                    {
                        identity = queryIdentity;
                        identityMap.Add(identity.Id, identity);
                    }
                    if (role != null)
                    {
                        identity.Roles.Add(role);
                    }
                    return identity;
                },
                param: parameters,
                splitOn: splitOn);
            return identities.Distinct().FirstOrDefault();
        }
    }
}
