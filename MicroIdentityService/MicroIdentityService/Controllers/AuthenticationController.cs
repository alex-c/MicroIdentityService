using MicroIdentityService.Controllers.Contracts.Requests;
using MicroIdentityService.Controllers.Contracts.Responses;
using MicroIdentityService.Services;
using MicroIdentityService.Services.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

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
        [HttpPost]
        public IActionResult AuthenticateUser([FromBody] AuthenticationRequest authenticationRequest)
        {
            if (authenticationRequest == null || 
                string.IsNullOrEmpty(authenticationRequest.Identifier) || 
                string.IsNullOrEmpty(authenticationRequest.Password))
            {
                return HandleBadRequest("An identity ID and password need to be supplied for authentication requests.");
            }

            try
            {
                if (AuthenticationService.TryAuthenticate(authenticationRequest.Identifier, authenticationRequest.Password, out string token))
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
    }
}
