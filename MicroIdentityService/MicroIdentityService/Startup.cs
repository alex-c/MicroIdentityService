using MicroIdentityService.Repositories;
using MicroIdentityService.Repositories.InMemory;
using MicroIdentityService.Services;
using MicroIdentityService.Services.IdentifierValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System;

namespace MicroIdentityService
{
    /// <summary>
    /// Encapsulates startup logic.
    /// </summary>
    public class Startup
    {
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
        public Startup(ILogger<Startup> logger, IConfiguration configuration)
        {
            Logger = logger;
            Configuration = configuration;
        }

        /// <summary>
        /// Configures app services.
        /// </summary>
        /// <param name="services">Service collection to extend.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            // Confiture persistence-related services
            ConfigurePersistenceServices(services);

            // Configure identifier validation
            ConfigureIdentifierValidation(services);

            // Register services
            services.AddSingleton<IdentifierValidationService>();
            services.AddSingleton<PasswordHashingService>();
            services.AddSingleton<AuthenticationService>();
            services.AddSingleton<IdentityService>();
            services.AddSingleton<DomainService>();

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
        private void ConfigureIdentifierValidation(IServiceCollection services)
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

            // Provide speicific identifier validator
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
        /// Configures the app.
        /// </summary>
        /// <param name="app">Application builder to configure the app through.</param>
        public void Configure(IApplicationBuilder app)
        {
            try
            {
                if (Configuration.GetValue<bool>("Administrator:CreateIfMissing"))
                {
                    string identifier = Configuration.GetValue<string>("Administrator:Identifier");
                    string password = Configuration.GetValue<string>("Administrator:Password");
                    IdentityService identityService = app.ApplicationServices.GetService<IdentityService>();
                    identityService.CreateIdentity(identifier, password);
                }
            }
            catch (Exception)
            {
                Logger.LogWarning("Failed processing `Administrator` configuration.");
            }

            app.UseSerilogRequestLogging();
            app.UseRouting();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
