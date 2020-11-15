using MicroIdentityService.Services.Exceptions;

namespace MicroIdentityService.Services.IdentifierValidation
{
    /// <summary>
    /// Indicates a failure to validate a user-chosen identifier.
    /// </summary>
    public class IdentifierValidationException : ValidationException
    {
        /// <summary>
        /// Instantiates an instance of <see cref="PasswordValidationException"/>.
        /// </summary>
        public IdentifierValidationException() : base("Identifier validation failed.") { }

        /// <summary>
        /// Instantiates an instance of <see cref="PasswordValidationException"/> with a specific error message.
        /// </summary>
        public IdentifierValidationException(string errorMessage) : base($"Identifier validation failed: {errorMessage}") { }
    }
}
