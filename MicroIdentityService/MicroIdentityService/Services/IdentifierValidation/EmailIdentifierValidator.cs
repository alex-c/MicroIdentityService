using System;
using System.Net.Mail;

namespace MicroIdentityService.Services.IdentifierValidation
{
    /// <summary>
    /// Validator that checks whether user-chosen identifiers are email addresses.
    /// </summary>
    public class EmailIdentifierValidator : IIdentifierValidator
    {
        /// <summary>
        /// Checks whether the provided identifier is an email addrerss.
        /// </summary>
        /// <param name="identifier">The identifier to check.</param>
        /// <returns>Returns whether the identifier is an email address.</returns>
        public bool Validate(string identifier)
        {
            try
            {
                new MailAddress(identifier);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
