namespace MicroIdentityService.Controllers.Contracts.Requests
{
    /// <summary>
    /// A contract for an identity creation request.
    /// </summary>
    public class IdentityCreationRequest
    {
        /// <summary>
        /// The identity's user-chosen identifier, used to log in.
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// The identity's as-of-yet unhashed password.
        /// </summary>
        public string Password { get; set; }
    }
}
