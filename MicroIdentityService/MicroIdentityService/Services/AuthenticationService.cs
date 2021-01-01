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
        private IdentityService IdentityService{ get; }

        /// <summary>
        /// Grants access to domain names.
        /// </summary>
        private DomainService DomainService { get; }

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
        /// <param name="identityService">Provides identities.</param>
        /// <param name="domainService">Provides domain names.</param>
        /// <param name="configuration">App configuration for JWT signing information.</param>
        public AuthenticationService(ILoggerFactory loggerFactory,
            PasswordHashingService passwordHashingService,
            IdentityService identityService,
            DomainService domainService,
            IConfiguration configuration)
        {
            Logger = loggerFactory.CreateLogger<AuthenticationService>();
            PasswordHashingService = passwordHashingService;
            IdentityService = identityService;
            DomainService = domainService;

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
        /// <exception cref="EntityNotFoundException">Thrown if no matching identity or any of the role domains could be found.</exception>
        public bool TryAuthenticate(string identifier, string password, out string serializedToken)
        {
            serializedToken = null;

            // Get identities, throws if not found
            Identity identity = IdentityService.GetIdentity(identifier);

            // Reject authentication attempt if identity is disabled
            if (identity.Disabled)
            {
                return false;
            }

            // Reject authentication attempt if bad password provided
            if (identity.HashedPassword != PasswordHashingService.HashAndSaltPassword(password, identity.Salt))
            {
                return false;
            }

            // Set identity base claims
            List<Claim> claims = new List<Claim>
            {
                // Add subject, name, role
                new Claim(JwtRegisteredClaimNames.Sub, identity.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, identity.Identifier),
            };

            // Add identity roles to claims
            claims.AddRange(GenerateRoleClaims(identity.Roles));

            // Generate token
            serializedToken = GenerateNewToken(claims);

            // Done!
            return true;
        }

        /// <summary>
        /// Refreshes a JWT token by generating a new token with the same claims and a refreshed expiration timestamp.
        /// </summary>
        /// <param name="token">The token to refresh.</param>
        /// <param name="refreshedToken">The refreshed token.</param>
        /// <returns>Returns whether the operation was successful.</returns>
        public bool TryRefresh(string token, out string refreshedToken)
        {
            refreshedToken = null;
            try
            {
                JwtSecurityToken deseiralizedToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
                refreshedToken = GenerateNewToken(deseiralizedToken.Claims);
                return true;
            }
            catch (Exception exception)
            {
                Logger.LogError(exception, "Token refresh failed for token: {0}", token);
                return false;
            }
        }

        /// <summary>
        /// Generates a list of `role` claims for the provided identity roles.
        /// </summary>
        /// <param name="roles">Roles to generate claims for.</param>
        /// <returns>Returns the newly generated claims.</returns>
        /// <exception cref="EntityNotFoundException">Thrown if any of the referenced domains could be found.</exception>
        private IEnumerable<Claim> GenerateRoleClaims(IEnumerable<Role> roles)
        {
            Dictionary<Guid, Domain> domains = new Dictionary<Guid, Domain>();
            List<Claim> claims = new List<Claim>();

            // For each role, retrieve domain if neccessary, and set claim with role name `[<domainName>.]<roleName>`.
            foreach (Role role in roles)
            {
                string roleName = "";
                if (role.DomainId.HasValue)
                {
                    Guid domainId = role.DomainId.Value;
                    if (!domains.ContainsKey(domainId))
                    {
                        domains.Add(domainId, DomainService.GetDomain(domainId));
                    }
                    roleName += domains[domainId].Name + ".";
                }
                roleName += role.Name;
                claims.Add(new Claim("role", roleName));
            }

            // Done, return role claims
            return claims;
        }

        /// <summary>
        /// Generates a new JWT with a given list of claims.
        /// </summary>
        /// <param name="claims">The claims to add to the token.</param>
        /// <returns>Returns the newly created token.</returns>
        private string GenerateNewToken(IEnumerable<Claim> claims)
        {
            JwtSecurityToken token = new JwtSecurityToken(JwtIssuer, null, claims, expires: DateTime.Now.Add(JwtLifetime), signingCredentials: SigningCredentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
