namespace MicroIdentityService.Controllers.Contracts.Requests
{
    /// <summary>
    /// A contract for an identity creation request.
    /// </summary>
    public class IdentityCreationRequest
    {
        /// <summary>
        /// The user's email address, used to log in.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The user's as-of-yet unhashed password.
        /// </summary>
        public string Password { get; set; }
    }
}
