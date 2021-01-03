using MicroIdentityService.Models;
using MicroIdentityService.Services;
using MicroIdentityService.Services.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MicroIdentityService
{
    /// <summary>
    /// A hosted service to performa setup on startup. This configures MIS domains, roles and administrative accounts.
    /// </summary>
    public class SetupService : IHostedService
    {
        private static readonly string MIS_DOMAIN_NAME = "mis";
        private static readonly string MIS_ADMINISTRATOR_ROLE_NAME = "admin";

        private IServiceProvider ServiceProvider { get; }
        private IConfiguration Configuration { get; }

        private ILogger Logger { get; }

        public SetupService(ILogger<SetupService> logger, IServiceProvider serviceProvider, IConfiguration configuration)
        {
            ServiceProvider = serviceProvider;
            Configuration = configuration;
            Logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation("Starting setup...");

            // Make sure that the MIS domain and admin role are present
            Logger.LogInformation("Ensuring the MIS domain and administrator role exist...");
            (Domain domain, Role adminRole) misEntities = await ConfigureMisDomainAndAdminRole();

            // If configured, ensure existense of an MIS admin
            await ConfigureAdministratorAccount(misEntities);

            Logger.LogInformation("Setup done.");
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        /// <summary>
        /// Ensures that the MIS domain and admin role are present - creating them if needed. Returns both.
        /// </summary>
        /// <returns>Returns the MIS domain and admin role entities.</returns>
        private async Task<(Domain, Role)> ConfigureMisDomainAndAdminRole()
        {
            // Get domains
            DomainService domainService = ServiceProvider.GetService<DomainService>();
            IEnumerable<Domain> domains = await domainService.GetDomains();

            // Configure MIS domain if needed
            Domain misDomain = domains.Where(d => d.Name == MIS_DOMAIN_NAME).FirstOrDefault();
            if (misDomain == null)
            {
                misDomain = await domainService.CreateDomain(MIS_DOMAIN_NAME);
            }

            // Get roles of MIS domain
            RoleService roleService = ServiceProvider.GetService<RoleService>();
            IEnumerable<Role> roles = await roleService.GetRoles(null, misDomain.Id);
            Role misAdminRole = roles.Where(r => r.Name == MIS_ADMINISTRATOR_ROLE_NAME).FirstOrDefault();
            if (misAdminRole == null)
            {
                misAdminRole = await roleService.CreateRole(MIS_ADMINISTRATOR_ROLE_NAME, misDomain.Id);
            }

            // Return MIS domain and admin role
            return (misDomain, misAdminRole);
        }

        /// <summary>
        /// If configured, this ensures an administrator account is present - creating it if needed.
        /// </summary>
        /// <param name="misEntities">The MIS domain and administrator role.</param>
        private async Task ConfigureAdministratorAccount((Domain domain, Role adminRole) misEntities)
        {
            try
            {
                if (Configuration.GetValue(ConfigurationPaths.CREATE_ADMINISTRATOR_IF_MISSING, false))
                {
                    string identifier = Configuration.GetValue<string>(ConfigurationPaths.ADMINISTRATOR_IDENTIFIER);
                    string password = Configuration.GetValue<string>(ConfigurationPaths.ADMINISTRATOR_PASSWORD);
                    if (string.IsNullOrWhiteSpace(identifier) || string.IsNullOrWhiteSpace(password))
                    {
                        throw new Exception("Invalid administrator identifier and/or password.");
                    }

                    Logger.LogInformation($"Ensuring administrator account '{identifier}' exists...");
                    IdentityService identityService = ServiceProvider.GetService<IdentityService>();
                    try
                    {
                        Identity admin = await identityService.GetIdentity(identifier);
                        Logger.LogInformation("Administrator account exists already.");
                    }
                    catch (EntityNotFoundException)
                    {
                        Logger.LogInformation($"Creating administrator account '{identifier}'.");
                        Identity admin = await identityService.CreateIdentity(identifier, password);
                        await identityService.UpdateIdentityRoles(admin.Id, new List<Guid>() { misEntities.adminRole.Id });
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.LogWarning($"Failed processing `Administrator` configuration: {exception.Message}");
            }
        }
    }
}
