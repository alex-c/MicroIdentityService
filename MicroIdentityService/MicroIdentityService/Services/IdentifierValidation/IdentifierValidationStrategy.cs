namespace MicroIdentityService.Services.IdentifierValidation
{
    /// <summary>
    /// Indicates a strategy for user-chosen identifier validation.
    /// </summary>
    public enum IdentifierValidationStrategy
    {
        /// <summary>
        /// Indicates that no validation is used.
        /// </summary>
        None,

        /// <summary>
        /// Indicates that identifiers are expected to be Email addresses.
        /// </summary>
        Email
    }
}
