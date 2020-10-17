using System;

namespace MicroIdentityService.Services.Exceptions
{
    /// <summary>
    /// Indicates a failure to validate a user-chosen identifier.
    /// </summary>
    public class IdentifierValidationException : Exception
    {
        /// <summary>
        /// Instantiates an instance of <see cref="IdentifierValidationException"/>.
        /// </summary>
        public IdentifierValidationException() : base("Identifier validation failed.") { }
    }
}
