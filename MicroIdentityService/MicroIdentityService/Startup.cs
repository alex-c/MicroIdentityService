using MicroIdentityService.Models;
using MicroIdentityService.Repositories;
using MicroIdentityService.Repositories.InMemory;
using MicroIdentityService.Services;
using MicroIdentityService.Services.IdentifierValidation;
using MicroIdentityService.Services.IdentifierValidation.Validators;
using MicroIdentityService.Services.PasswordValidation;
using MicroIdentityService.Services.PasswordValidation.Validators;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MicroIdentityService
{
    /// <summary>
    /// Encapsulates startup logic.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// CORS policy name for local development.
        /// </summary>
        private static readonly string LOCAL_DEVELOPMENT_CORS_POLICY = "localDevelopmentCorsPolicy";
        private static readonly string MIS_DOMAIN_NAME = "mis";
        private static readonly string MIS_ADMINISTRATOR_ROLE_NAME = "admin";

        /// <summary>
        /// The hosting environment information.
        /// </summary>
        private IWebHostEnvironment Environment { get; }

        /// <summary>
        /// The app configuration.
        /// </summary>
        private IConfiguration Configuration { get; }

        /// <summary>
        /// Logger instance for local logging needs.
        /// </summary>
        private Microsoft.Extensions.Logging.ILogger Logger { get; }

        /// <summary>
        /// Injects modules necessary to app and services configuration.
        /// </summary>
        /// <param name="logger">Logger instance for local logging needs.</param>
        /// <param name="configuration">App configuration.</param>
        /// <param name="environemnt">Hosting environment.</param>
        public Startup(ILogger<Startup> logger, IConfiguration configuration, IWebHostEnvironment environemnt)
        {
            Logger = logger;
            Configuration = configuration;
            Environment = environemnt;
        }

        /// <summary>
        /// Configures app services.
        /// </summary>
        /// <param name="services">Service collection to extend.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            // Configure CORS
            services.AddCors(options =>
            {
                if (Environment.IsDevelopment())
                {
                    options.AddPolicy(LOCAL_DEVELOPMENT_CORS_POLICY, builder =>
                    {
                        builder.WithOrigins("http://localhost:8080")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                    });
                }
            });

            // Confiture persistence-related services
            ConfigurePersistenceServices(services);

            // Configure identifier and password validation
            ConfigureIdentifierValidationService(services);
            ConfigurePasswordValidationService(services);

            // Register services
            services.AddSingleton<IdentifierValidationService>();
            services.AddSingleton<PasswordValidationService>();
            services.AddSingleton<PasswordHashingService>();
            services.AddSingleton<AuthenticationService>();
            services.AddSingleton<IdentityService>();
            services.AddSingleton<DomainService>();
            services.AddSingleton<RoleService>();

            // Register controllers
            services.AddControllers();
        }

        /// <summary>
        /// Configures persistence-related services. This provides implementations for entity repositories.
        /// </summary>
        /// <param name="services">Service collection to register services in.</param>
        private void ConfigurePersistenceServices(IServiceCollection services)
        {
            // Get persistence strategy from configuration
            PersistenceStrategy persistenceStrategy;
            try
            {
                persistenceStrategy = Configuration.GetValue<PersistenceStrategy>("Persistence:Strategy");
            }
            catch (Exception exception)
            {
                throw new Exception("Failed getting persistence strategy from configuration.", exception);
            }
            Logger.LogInformation($"Persistence strategy `{persistenceStrategy}` has been configured.");

            // Register repositories
            if (persistenceStrategy == PersistenceStrategy.InMemory)
            {
                services.AddSingleton<InMemoryIdentityRepository>();
                services.AddSingleton<IIdentityRepository>(x => x.GetRequiredService<InMemoryIdentityRepository>());
                services.AddSingleton<IReadOnlyIdentityRepository>(x => x.GetRequiredService<InMemoryIdentityRepository>());
                services.AddSingleton<IDomainRepository, InMemoryDomainRepository>();
                services.AddSingleton<IRoleRepository, InMemoryRoleRepository>();
            }
            else
            {
                // TODO: implement real DB persistence
                throw new NotImplementedException("Real database persistence has not been implemented yet.");
            }
        }

        /// <summary>
        /// Configures identifier validation. This provides an implementation for the <see cref="IIdentifierValidator"/> interface.
        /// </summary>
        /// <param name="services">Service collection to register services in.</param>
        private void ConfigureIdentifierValidationService(IServiceCollection services)
        {
            // Get identifier validation strategy from configuration
            IdentifierValidationStrategy identifierValidationStrategy;
            try
            {
                identifierValidationStrategy = Configuration.GetValue<IdentifierValidationStrategy>("IdentifierValidation:Strategy");
            }
            catch (Exception)
            {
                Logger.LogWarning("Failed getting identifier validation strategy from configuration - defaulting to no validation.");
                identifierValidationStrategy = IdentifierValidationStrategy.None;
            }
            Logger.LogInformation($"Identifier validation strategy `{identifierValidationStrategy}` has been configured.");

            // Provide speicific password validator
            switch (identifierValidationStrategy)
            {
                case IdentifierValidationStrategy.Email:
                    services.AddSingleton<IIdentifierValidator, EmailIdentifierValidator>();
                    break;
                case IdentifierValidationStrategy.None:
                default:
                    services.AddSingleton<IIdentifierValidator, NoOpIdentifierValidator>();
                    break;
            }
        }

        /// <summary>
        /// Configures password validation. This provides an implementation for the <see cref="IPasswordValidator"/> interface.
        /// </summary>
        /// <param name="services">Service collection to register services in.</param>
        private void ConfigurePasswordValidationService(IServiceCollection services)
        {
            // Get password validation strategy from configuration
            PasswordValidationStrategy passwordValidationStrategy;
            try
            {
                passwordValidationStrategy = Configuration.GetValue<PasswordValidationStrategy>("PasswordValidation:Strategy");
            }
            catch (Exception)
            {
                Logger.LogWarning("Failed getting password validation strategy from configuration - defaulting to no validation.");
                passwordValidationStrategy = PasswordValidationStrategy.None;
            }
            Logger.LogInformation($"Password validation strategy `{passwordValidationStrategy}` has been configured.");

            // Provide speicific identifier validator
            switch (passwordValidationStrategy)
            {
                case PasswordValidationStrategy.MinimumLength:
                    services.AddSingleton<IPasswordValidator, MinimumLengthPasswordValidator>();
                    break;
                case PasswordValidationStrategy.None:
                default:
                    services.AddSingleton<IPasswordValidator, NoOpPasswordValidator>();
                    break;
            }
        }

        /// <summary>
        /// Configures the app.
        /// </summary>
        /// <param name="app">Application builder to configure the app through.</param>
        public void Configure(IApplicationBuilder app)
        {
            // Make sure that the MIS domain and admin role are present
            (Domain domain, Role adminRole) misEntities = ConfigureMisDomainAndAdminRole(app);

            // If configured, ensure existense of an MIS admin
            try
            {
                if (Configuration.GetValue("Administrator:CreateIfMissing", false))
                {
                    string identifier = Configuration.GetValue<string>("Administrator:Identifier");
                    string password = Configuration.GetValue<string>("Administrator:Password");
                    IdentityService identityService = app.ApplicationServices.GetService<IdentityService>();
                    Identity identity = identityService.CreateIdentity(identifier, password);
                    identity.Roles = new List<Role>() { misEntities.adminRole };
                }
            }
            catch (Exception exception)
            {
                Logger.LogWarning($"Failed processing `Administrator` configuration: {exception.Message}");
            }

            app.UseSerilogRequestLogging();
            app.UseRouting();

            app.UseCors(LOCAL_DEVELOPMENT_CORS_POLICY);

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }

        /// <summary>
        /// Ensures that the MIS domain and admin role are present - creating them if needed. Returns both.
        /// </summary>
        /// <param name="app">Grants access to app services.</param>
        /// <returns>Returns the MIS domain and admin role entities.</returns>
        private (Domain, Role) ConfigureMisDomainAndAdminRole(IApplicationBuilder app)
        {
            // Get domains
            DomainService domainService = app.ApplicationServices.GetService<DomainService>();
            IEnumerable<Domain> domains = domainService.GetDomains();

            // Configure MIS domain if needed
            Domain misDomain = domains.Where(d => d.Name == MIS_DOMAIN_NAME).FirstOrDefault();
            if (misDomain == null)
            {
                misDomain = domainService.CreateDomain(MIS_DOMAIN_NAME);
            }

            // Get roles of MIS domain
            RoleService roleService = app.ApplicationServices.GetService<RoleService>();
            IEnumerable<Role> roles = roleService.GetRoles("", misDomain.Id);
            Role misAdminRole = roles.Where(r => r.Name == MIS_ADMINISTRATOR_ROLE_NAME).FirstOrDefault();
            if (misAdminRole == null)
            {
                misAdminRole = roleService.CreateRole(MIS_ADMINISTRATOR_ROLE_NAME, misDomain.Id);
            }

            // Return MIS domain and admin role
            return (misDomain, misAdminRole);
        }
    }
}
