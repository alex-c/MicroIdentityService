namespace MicroIdentityService.Controllers.Contracts.Requests
{
    /// <summary>
    /// A contract for a request to change an identity's password.
    /// </summary>
    public class PasswordChangeRequest
    {
        /// <summary>
        /// The old password for verification purposes.
        /// </summary>
        public string OldPassword { get; set; }

        /// <summary>
        /// The new password to set.
        /// </summary>
        public string NewPassword { get; set; }
    }
}
