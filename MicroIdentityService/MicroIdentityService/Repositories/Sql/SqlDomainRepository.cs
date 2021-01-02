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
    public class SqlDomainRepository : BaseSqlRepository, IDomainRepository
    {
        public SqlDomainRepository(IConfiguration configuration, ILogger<SqlDomainRepository> logger) : base(configuration, logger) { }

        public async Task<IEnumerable<Domain>> GetDomains()
        {
            string sql = "SELECT * FROM domains d LEFT JOIN roles r ON r.domain_id = d.id";
            return await QueryMultipleDomains(sql, null);
        }

        public async Task<IEnumerable<Domain>> SearchDomainsByName(string filter)
        {
            string sql = "SELECT * FROM domains d LEFT JOIN roles r ON r.domain_id = d.id WHERE name LIKE @Filter";
            return await QueryMultipleDomains(sql, new { Filter = $"%{filter}%" });
        }

        public async Task<Domain> GetDomain(Guid id)
        {
            Domain domain = null;
            using (IDbConnection connection = GetNewConnection())
            {
                domain = await QuerySingleDomain(connection, id);
            }
            return domain;
        }

        public async Task<Domain> GetDomain(string name)
        {
            string sql = "SELECT * FROM domains d LEFT JOIN roles r ON r.domain_id = d.id WHERE d.name=@Name";

            Domain domain = null;
            using (IDbConnection connection = GetNewConnection())
            {
                domain = await QuerySingleDomain(connection, sql, new { name }, "name");
            }
            return domain;
        }

        public async Task<Domain> CreateDomain(string name)
        {
            Guid id = Guid.NewGuid();

            Domain domain = null;
            using (IDbConnection connection = GetNewConnection())
            {
                await connection.ExecuteAsync("INSERT INTO domains (id, name) VALUES (@Id, @Name)", new { id, name });
                domain = await QuerySingleDomain(connection, id);
            }
            return domain;
        }

        public async Task<Domain> UpdateDomain(Domain domain)
        {
            using (IDbConnection connection = GetNewConnection())
            {
                await connection.ExecuteAsync("UPDATE domains SET name=@Name WHERE id=@Id", new { domain.Id, domain.Name });
            }
            return domain;
        }

        public async Task DeleteDomain(Guid id)
        {
            using (IDbConnection connection = GetNewConnection())
            {
                await connection.ExecuteAsync("DELETE FROM domains WHERE id=@Id", new { id });
            }
        }

        private async Task<IEnumerable<Domain>> QueryMultipleDomains(string sql, object parameters)
        {
            IEnumerable<Domain> domains = null;
            Dictionary<Guid, Domain> domainMap = new Dictionary<Guid, Domain>();
            using (IDbConnection connection = GetNewConnection())
            {
                domains = await connection.QueryAsync<Domain, Role, Domain>(sql,
                    (queryDomain, role) =>
                    {
                        Domain domain = null;
                        if (!domainMap.TryGetValue(queryDomain.Id, out domain))
                        {
                            domain = queryDomain;
                            domainMap.Add(domain.Id, domain);
                        }
                        if (role != null)
                        {
                            domain.Roles.Add(role);
                        }
                        return domain;
                    },
                    param: parameters,
                    splitOn: "id");
            }
            return domains.Distinct();
        }

        private async Task<Domain> QuerySingleDomain(IDbConnection connection, Guid id)
        {
            string sql = "SELECT * FROM domains d LEFT JOIN roles r ON r.domain_id = d.id WHERE d.id=@Id";
            return await QuerySingleDomain(connection, sql, new { id });
        }

        private async Task<Domain> QuerySingleDomain(IDbConnection connection, string sql, object parameters, string splitOn = "domain_id")
        {
            IEnumerable<Domain> domains = null;
            Dictionary<Guid, Domain> domainMap = new Dictionary<Guid, Domain>();
            domains = await connection.QueryAsync<Domain, Role, Domain>(sql,
                (queryDomain, role) =>
                {
                    Domain domain = null;
                    if (!domainMap.TryGetValue(queryDomain.Id, out domain))
                    {
                        domain = queryDomain;
                        domainMap.Add(domain.Id, domain);
                    }
                    if (role != null)
                    {
                        domain.Roles.Add(role);
                    }
                    return domain;
                },
                param: parameters,
                splitOn: splitOn);
            return domains.Distinct().FirstOrDefault();
        }

    }
}
