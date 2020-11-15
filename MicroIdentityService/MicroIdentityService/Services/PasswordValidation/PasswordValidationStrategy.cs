namespace MicroIdentityService.Services.PasswordValidation
{
    /// <summary>
    /// Indicates a strategy for user-chosen password validation.
    /// </summary>
    public enum PasswordValidationStrategy
    {
        /// <summary>
        /// Indicates that no validation is used.
        /// </summary>
        None,

        /// <summary>
        /// Indicates that passwords are expected to have a minimum length.
        /// </summary>
        MinimumLength
    }
}
