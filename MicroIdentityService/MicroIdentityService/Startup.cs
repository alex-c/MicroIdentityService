using MicroIdentityService.Authorization;
using MicroIdentityService.Repositories;
using MicroIdentityService.Repositories.InMemory;
using MicroIdentityService.Repositories.Sql;
using MicroIdentityService.Repositories.Sql.Support;
using MicroIdentityService.Services;
using MicroIdentityService.Services.IdentifierValidation;
using MicroIdentityService.Services.IdentifierValidation.Validators;
using MicroIdentityService.Services.PasswordValidation;
using MicroIdentityService.Services.PasswordValidation.Validators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System;
using System.Text;

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

            // Check JWT signing key validity
            if (Configuration.GetValue<string>(ConfigurationPaths.JWT_SECRET).Length < 16)
            {
                string errorMessage = "The secret for signing JWTs has to be at least 16 characters long.";
                Logger.LogError(errorMessage);
                throw new Exception(errorMessage);
            }

            // Configure JWT-based auth
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateLifetime = true,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration.GetValue<string>(ConfigurationPaths.JWT_ISSUER),
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetValue<string>(ConfigurationPaths.JWT_SECRET)))
                    };
                });

            // Register authorization policies
            services.AddAuthorization(options =>
            {
                // API Keys
                RegisterPolicy(options, Policies.API_KEYS_GET, Permissions.API_KEYS_GET);
                RegisterPolicy(options, Policies.API_KEYS_CREATE, Permissions.API_KEYS_CREATE);
                RegisterPolicy(options, Policies.API_KEYS_UPDATE, Permissions.API_KEYS_UPDATE);
                RegisterPolicy(options, Policies.API_KEYS_DELETE, Permissions.API_KEYS_DELETE);

                // Domains
                RegisterPolicy(options, Policies.DOMAINS_GET, Permissions.DOMAINS_GET);
                RegisterPolicy(options, Policies.DOMAINS_CREATE, Permissions.DOMAINS_CREATE);
                RegisterPolicy(options, Policies.DOMAINS_UPDATE, Permissions.DOMAINS_UPDATE);
                RegisterPolicy(options, Policies.DOMAINS_DELETE, Permissions.DOMAINS_DELETE);

                // Users
                RegisterPolicy(options, Policies.IDENTITIES_GET, Permissions.IDENTITIES_GET);
                RegisterPolicy(options, Policies.IDENTITIES_CREATE, Permissions.IDENTITIES_CREATE);
                RegisterPolicy(options, Policies.IDENTITIES_UPDATE, Permissions.IDENTITIES_UPDATE);
                RegisterPolicy(options, Policies.IDENTITIES_DELETE, Permissions.IDENTITIES_DELETE);
                RegisterPolicy(options, Policies.IDENTITIES_GET_ROLES, Permissions.IDENTITIES_GET_ROLES);
                RegisterPolicy(options, Policies.IDENTITIES_SET_ROLES, Permissions.IDENTITIES_SET_ROLES);

                // Roles
                RegisterPolicy(options, Policies.ROLES_GET, Permissions.ROLES_GET);
                RegisterPolicy(options, Policies.ROLES_CREATE, Permissions.ROLES_CREATE);
                RegisterPolicy(options, Policies.ROLES_UPDATE, Permissions.ROLES_UPDATE);
                RegisterPolicy(options, Policies.ROLES_DELETE, Permissions.ROLES_DELETE);
            });

            // Register authorization handlers
            services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, AdministratorAuthorizationHandler>();

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
            services.AddSingleton<ApiKeyService>();

            // Register controllers
            services.AddControllers();

            // Setup service to run on start
            services.AddHostedService<SetupService>();
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
                persistenceStrategy = Configuration.GetValue<PersistenceStrategy>(ConfigurationPaths.PERSISTENCE_STRATEGY);
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
                services.AddSingleton<IIdentityRepository, InMemoryIdentityRepository>();
                services.AddSingleton<IDomainRepository, InMemoryDomainRepository>();
                services.AddSingleton<IRoleRepository, InMemoryRoleRepository>();
                services.AddSingleton<IIdentityRoleRepository, InMemoryIdentityRolesRepository>();
                services.AddSingleton<IApiKeyRepository, InMemoryApiKeyRepository>();
            }
            else if (persistenceStrategy == PersistenceStrategy.Sql)
            {
                // Configure Dapper to convert `column_name` to property `ColumnName` and handle UTC times right
                Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
                Dapper.SqlMapper.AddTypeHandler(new DateTimeHandler());

                services.AddSingleton<IApiKeyRepository, SqlApiKeyRepository>();
                services.AddSingleton<IDomainRepository, SqlDomainRepository>();
                services.AddSingleton<IRoleRepository, SqlRoleRepository>();
                services.AddSingleton<IIdentityRepository, SqlIdentityRepository>();
                services.AddSingleton<IIdentityRoleRepository, SqlIdentityRoleRepository>();
            }
            else
            {
                throw new NotImplementedException($"The persistence strategy `{persistenceStrategy}` has not been implemented.");
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
                identifierValidationStrategy = Configuration.GetValue<IdentifierValidationStrategy>(ConfigurationPaths.IDENTIFIER_VALIDATION_STRATEGY);
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
                passwordValidationStrategy = Configuration.GetValue<PasswordValidationStrategy>(ConfigurationPaths.PASSWORD_VALIDATION_STRATEGY);
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
        /// Registers an authorization policy for agiven permission.
        /// </summary>
        /// <param name="options">The authorization options to add a new policy to.</param>
        /// <param name="policyName">The name of the new policy.</param>
        /// <param name="permission">The permission this policy is for.</param>
        private void RegisterPolicy(AuthorizationOptions options, string policyName, string permission)
        {
            options.AddPolicy(policyName, policy => policy
                .RequireAuthenticatedUser()
                .Requirements.Add(new PermissionAuthorizationRequirement(permission)));
        }

        /// <summary>
        /// Configures the middleware pipleine.
        /// </summary>
        /// <param name="app">Application builder to configure the app through.</param>
        public void Configure(IApplicationBuilder app)
        {
            app.UseSerilogRequestLogging();
            app.UseRouting();

            app.UseCors(LOCAL_DEVELOPMENT_CORS_POLICY);

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }
}
