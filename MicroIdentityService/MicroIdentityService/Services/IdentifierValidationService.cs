using MicroIdentityService.Services.Exceptions;
using MicroIdentityService.Services.IdentifierValidation;
using Microsoft.Extensions.Logging;

namespace MicroIdentityService.Services
{
    /// <summary>
    /// Provides validation of user-chosen identifiers.
    /// </summary>
    public class IdentifierValidationService
    {
        /// <summary>
        /// Provides the actual validation logic used by this service.
        /// </summary>
        private IIdentifierValidator IdentifierValidator { get; }

        /// <summary>
        /// Logger instance for local logging needs.
        /// </summary>
        private ILogger Logger { get; }

        public IdentifierValidationService(ILogger<IdentifierValidationService> logger, IIdentifierValidator identifierValidator)
        {
            IdentifierValidator = identifierValidator;
            Logger = logger;
        }

        /// <summary>
        /// Attempts to validate a user-provided identifier.
        /// </summary>
        /// <param name="identifier">The identifier to validate.</param>
        /// <exception cref="IdentifierValidationException">Thrown on validation failure.</exception>
        public void Validate(string identifier)
        {
            if (!IdentifierValidator.Validate(identifier))
            {
                throw new IdentifierValidationException();
            }
        }

    }
}
