using MicroIdentityService.Models;
using MicroIdentityService.Repositories;
using MicroIdentityService.Services.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MicroIdentityService.Services
{
    /// <summary>
    /// A service that provides authentication functionality.
    /// </summary>
    public class AuthenticationService
    {
        /// <summary>
        /// Provides password hashing functionalities.
        /// </summary>
        private PasswordHashingService PasswordHashingService { get; }

        /// <summary>
        /// Grants access to identities.
        /// </summary>
        private IReadOnlyIdentityRepository IdentityRepository { get; }

        /// <summary>
        /// Signing credentials for JWTs.
        /// </summary>
        private SigningCredentials SigningCredentials { get; }

        /// <summary>
        /// Lifetime of issued JWTs.
        /// </summary>
        private TimeSpan JwtLifetime { get; }

        /// <summary>
        /// Issuer name of issued JWTs.
        /// </summary>
        private string JwtIssuer { get; }

        /// <summary>
        /// A logger for local logging needs.
        /// </summary>
        private ILogger Logger { get; }

        /// <summary>
        /// Sets up the service with all needed dependencies.
        /// </summary>
        /// <param name="loggerFactory">Factory to create loggers from.</param>
        /// <param name="passwordHashingService">Provides password hashing functionalities.</param>
        /// <param name="identityRepository">Provides identities.</param>
        /// <param name="configuration">App configuration for JWT signing information.</param>
        public AuthenticationService(ILoggerFactory loggerFactory,
            PasswordHashingService passwordHashingService,
            IReadOnlyIdentityRepository identityRepository,
            IConfiguration configuration)
        {
            Logger = loggerFactory.CreateLogger<AuthenticationService>();
            PasswordHashingService = passwordHashingService;
            IdentityRepository = identityRepository;

            // JWT-related configuration
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("Jwt:Secret")));
            SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            JwtLifetime = TimeSpan.FromMinutes(configuration.GetValue<int>("Jwt:LifetimeInMinutes"));
            JwtIssuer = configuration.GetValue<string>("Jwt:Issuer");
        }

        /// <summary>
        /// Attempts to authenticate an identity.
        /// </summary>
        /// <param name="identifier">The identity's unique identifier.</param>
        /// <param name="password">The identity's password.</param>
        /// <param name="serializedToken">The serialized token.</param>
        /// <returns>Returns whether authentication was successful.</returns>
        /// <exception cref="EntityNotFoundException">User</exception>
        public bool TryAuthenticate(string identifier, string password, out string serializedToken)
        {
            serializedToken = null;

            // Get user
            Identity identity = IdentityRepository.GetIdentity(identifier);
            if (identity == null)
            {
                throw new EntityNotFoundException("Identity", identifier);
            }

            // Check password
            if (identity.HashedPassword != PasswordHashingService.HashAndSaltPassword(password, identity.Salt))
            {
                return false;
            }

            // Set user claims
            List<Claim> claims = new List<Claim>
            {
                // Add subject, name, role
                new Claim(JwtRegisteredClaimNames.Sub, identity.Id.ToString()),
            };

            // Generate token
            JwtSecurityToken token = new JwtSecurityToken(JwtIssuer, null, claims, expires: DateTime.Now.Add(JwtLifetime), signingCredentials: SigningCredentials);
            serializedToken = new JwtSecurityTokenHandler().WriteToken(token);

            // Done!
            return true;
        }
    }
}
