namespace MicroIdentityService.Services.IdentifierValidation
{
    /// <summary>
    /// A generic interface for a component that provides validation logic for user-chose identifiers.
    /// </summary>
    public interface IIdentifierValidator
    {
        /// <summary>
        /// Attempts to validate a user-chosen identifier.
        /// </summary>
        /// <param name="identifier">The identifier to validate.</param>
        /// <param name="errorMessage">Error message, set in case of validation failure.</param>
        /// <returns>Returns whether validation was successful.</returns>
        bool Validate(string identifier, out string errorMessage);
    }
}
