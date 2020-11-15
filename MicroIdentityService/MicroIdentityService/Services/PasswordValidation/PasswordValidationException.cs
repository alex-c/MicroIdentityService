using MicroIdentityService.Services.Exceptions;

namespace MicroIdentityService.Services.PasswordValidation
{
    /// <summary>
    /// Indicates a failure to validate a user-chosen password.
    /// </summary>
    public class PasswordValidationException : ValidationException
    {
        /// <summary>
        /// Instantiates an instance of <see cref="PasswordValidationException"/>.
        /// </summary>
        public PasswordValidationException() : base("Password validation failed.") { }

        /// <summary>
        /// Instantiates an instance of <see cref="PasswordValidationException"/> with a specific error message.
        /// </summary>
        public PasswordValidationException(string errorMessage) : base($"Password validation failed: {errorMessage}") { }
    }
}
