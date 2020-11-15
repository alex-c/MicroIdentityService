using System;
using System.Net.Mail;

namespace MicroIdentityService.Services.IdentifierValidation.Validators
{
    /// <summary>
    /// Validator that checks whether user-chosen identifiers are email addresses.
    /// </summary>
    public class EmailIdentifierValidator : IIdentifierValidator
    {
        public bool Validate(string identifier, out string errorMessage)
        {
            errorMessage = null;
            try
            {
                new MailAddress(identifier);
                return true;
            }
            catch (FormatException)
            {
                errorMessage = "An identifier must be a valid e-mail address.";
                return false;
            }
        }
    }
}
