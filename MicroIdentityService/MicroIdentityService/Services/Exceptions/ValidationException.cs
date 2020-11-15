using System;

namespace MicroIdentityService.Services.Exceptions
{
    /// <summary>
    /// A base expception for failures of user-provided content validation.
    /// </summary>
    public class ValidationException : Exception
    {
        /// <summary>
        /// Creates an isntance of <see cref="ValidationException"/> with a specific error message.
        /// </summary>
        /// <param name="errorMessage">An error message describing the specific validation failure being described.</param>
        public ValidationException(string errorMessage) : base(errorMessage) { }
    }
}
