namespace MicroIdentityService.Controllers.Contracts.Requests
{
    /// <summary>
    /// A contract for a request to create or update a domain.
    /// </summary>
    public class DomainCreationOrUpdateRequest
    {
        /// <summary>
        /// The name of the domain to create or update.
        /// </summary>
        public string Name { get; set; }
    }
}
