using MicroIdentityService.Repositories;
using MicroIdentityService.Repositories.InMemory;
using MicroIdentityService.Services;
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
            }
            else
            {
                // TODO: implement real DB persistence
                throw new NotImplementedException("Real database persistence has not been implemented yet.");
            }

            // Register services
            services.AddSingleton<PasswordHashingService>();
            services.AddSingleton<AuthenticationService>();
            services.AddSingleton<IdentityService>();

            // Register controllers
            services.AddControllers();
        }

        /// <summary>
        /// Configures the app.
        /// </summary>
        /// <param name="app">Application builder to configure the app through.</param>
        public void Configure(IApplicationBuilder app)
        {
            app.UseSerilogRequestLogging();
            app.UseRouting();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
