using MicroIdentityService.Models;
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
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            if (Configuration.GetValue<string>("Jwt:Secret").Length < 16)
            {
                string errorMessage = "The secret for signing JWTs has to be at least 16 characters long.";
                Logger.LogError(errorMessage);
                throw new Exception(errorMessage);
            }

            // Configure JWT-based authorization
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateLifetime = true,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration.GetValue<string>("Jwt:Issuer"),
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetValue<string>("Jwt:Secret")))
                    };
                });
            services.AddAuthorization();

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
