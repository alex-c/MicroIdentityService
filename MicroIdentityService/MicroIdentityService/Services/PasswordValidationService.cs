using MicroIdentityService.Services.PasswordValidation;
using Microsoft.Extensions.Logging;

namespace MicroIdentityService.Services
{
    /// <summary>
    /// Provides validation of user-chosen passwords.
    /// </summary>
    public class PasswordValidationService
    {
        /// <summary>
        /// Provides the actual validation logic used by this service.
        /// </summary>
        private IPasswordValidator PasswordValidator { get; }

        /// <summary>
        /// Logger instance for local logging needs.
        /// </summary>
        private ILogger Logger { get; }

        public PasswordValidationService(ILogger<PasswordValidationService> logger, IPasswordValidator passwordValidator)
        {
            PasswordValidator = passwordValidator;
            Logger = logger;
        }

        /// <summary>
        /// Attempts to validate a user-provided password.
        /// </summary>
        /// <param name="password">The password to validate.</param>
        /// <exception cref="PasswordValidationException">Thrown on validation failure.</exception>
        public void Validate(string password)
        {
            if (!PasswordValidator.Validate(password, out string errorMessage))
            {
                throw new PasswordValidationException(errorMessage);
            }
        }

    }
}
