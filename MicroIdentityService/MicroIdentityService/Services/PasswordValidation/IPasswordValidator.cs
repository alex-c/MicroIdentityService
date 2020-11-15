namespace MicroIdentityService.Services.PasswordValidation
{
    /// <summary>
    /// A generic interface for a component that provides validation logic for user-chosen password.
    /// </summary>
    public interface IPasswordValidator
    {
        /// <summary>
        /// Attempts to validate a user-chosen password.
        /// </summary>
        /// <param name="password">The password to validate.</param>
        /// <param name="errorMessage">Error message, set in case of validation failure.</param>
        /// <returns>Returns whether validation was successful.</returns>
        bool Validate(string password, out string errorMessage);
    }
}
