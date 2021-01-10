using MicroIdentityService.Controllers.Contracts.Requests;
using MicroIdentityService.Controllers.Contracts.Responses;
using MicroIdentityService.Services;
using MicroIdentityService.Services.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System;
using System.Threading.Tasks;

namespace MicroIdentityService.Controllers
{
    /// <summary>
    /// API for identity authentication.
    /// </summary>
    [Route("api/v1/auth")]
    public class AuthenticationController : ControllerBase
    {
        /// <summary>
        /// The service providing authentication functionality.
        /// </summary>
        private AuthenticationService AuthenticationService { get; }

        /// <summary>
        /// Initializes a controller instance.
        /// </summary>
        /// <param name="logger">Logger for local logging needs.</param>
        /// <param name="authService">Injected authentication service.</param>
        public AuthenticationController(ILogger<AuthenticationController> logger, AuthenticationService authenticationService)
        {
            AuthenticationService = authenticationService;
            Logger = logger;
        }

        /// <summary>
        /// Authenticates an identity.
        /// </summary>
        /// <param name="authenticationRequest">Authentication request data.</param>
        /// <returns>Returns a JWT on success.</returns>
        [HttpPost("identity")]
        public async Task<IActionResult> AuthenticateUser([FromBody] IdentityAuthenticationRequest authenticationRequest)
        {
            if (authenticationRequest == null || 
                string.IsNullOrEmpty(authenticationRequest.Identifier) || 
                string.IsNullOrEmpty(authenticationRequest.Password))
            {
                return HandleBadRequest("An identity identifier and password need to be supplied for authentication requests.");
            }

            try
            {
                string token = await AuthenticationService.Authenticate(authenticationRequest.Identifier, authenticationRequest.Password);
                if (token != null)
                {
                    return Ok(new AuthenticationResponse(token));
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (EntityNotFoundException)
            {
                return Unauthorized();
            }
            catch (Exception exception)
            {
                return HandleUnexpectedException(exception);
            }
        }

        /// <summary>
        /// Authenticates a client application using an API key.
        /// </summary>
        /// <param name="authenticationRequest">Authentication request data.</param>
        /// <returns>Returns a JWT on success.</returns>
        [HttpPost("client")]
        public async Task<IActionResult> AuthenticateClient([FromBody] ClientAuthenticationRequest authenticationRequest)
        {
            if (authenticationRequest == null || authenticationRequest.ApiKey == null)
            {
                return HandleBadRequest("An API key needs to be supplied for client authentication requests.");
            }

            try
            {
                string token = await AuthenticationService.Authenticate(authenticationRequest.ApiKey);
                if (token != null)
                {
                    return Ok(new AuthenticationResponse(token));
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (EntityNotFoundException)
            {
                return Unauthorized();
            }
            catch (Exception exception)
            {
                return HandleUnexpectedException(exception);
            }
        }

        [HttpGet("refresh")]
        [Authorize]
        public IActionResult RefreshToken()
        {
            // Retrieve authorization header
            string authorizationHeader = Request.Headers[HeaderNames.Authorization].ToString();
            if (String.IsNullOrWhiteSpace(authorizationHeader) || !authorizationHeader.Contains("Bearer "))
            {
                return Unauthorized();
            }

            // Retrieve token from authorization header
            string token = authorizationHeader.Substring(8, authorizationHeader.Length - 8);
            if (String.IsNullOrWhiteSpace(token))
            {
                return Unauthorized();
            }

            // Attempt to refresh the token and return
            if (AuthenticationService.TryRefresh(token, out string refreshedToken))
            {
                return Ok(new AuthenticationResponse(refreshedToken));
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}
