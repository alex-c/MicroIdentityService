namespace MicroIdentityService.Controllers.Contracts.Responses
{
    /// <summary>
    /// A contract for a repsonse to a successful authentication request.
    /// </summary>
    public class AuthenticationResponse
    {
        /// <summary>
        /// JWT which will allow the authenticated client to access private API routes.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Creates a response to a successful authentication request.
        /// </summary>
        /// <param name="token">The JWT to deliver.</param>
        public AuthenticationResponse(string token)
        {
            Token = token;
        }
    }
}
