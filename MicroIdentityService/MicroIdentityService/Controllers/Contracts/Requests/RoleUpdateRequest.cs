namespace MicroIdentityService.Controllers.Contracts.Requests
{
    /// <summary>
    /// A contract for a request to update an existing role.
    /// </summary>
    public class RoleUpdateRequest
    {
        /// <summary>
        /// The new name to set for the role to update.
        /// </summary>
        public string Name { get; set; }
    }
}
