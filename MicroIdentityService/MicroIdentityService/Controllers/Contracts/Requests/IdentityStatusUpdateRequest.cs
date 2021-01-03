namespace MicroIdentityService.Controllers.Contracts.Requests
{
    /// <summary>
    /// A contract for a request to update an identity.
    /// </summary>
    public class IdentityStatusUpdateRequest
    {
        /// <summary>
        /// Indicates whether the identity is disabled.
        /// </summary>
        public bool Disabled { get; set; }
    }
}
