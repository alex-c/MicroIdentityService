using Microsoft.Extensions.Configuration;

namespace MicroIdentityService.Services.PasswordValidation.Validators
{
    /// <summary>
    /// A password validator that checks for a minimum length.
    /// </summary>
    public class MinimumLengthPasswordValidator : IPasswordValidator
    {
        /// <summary>
        /// The actual minimum length used for validation.
        /// </summary>
        private int MinimumLength;

        /// <summary>
        /// Sets up the validator with a minimum length provided by the app configuration. Defaults to 12.
        /// </summary>
        /// <param name="configuration">App configuration.</param>
        public MinimumLengthPasswordValidator(IConfiguration configuration)
        {
            MinimumLength = configuration.GetValue("PasswordValidation:Configuration:MinimumLength", 12);
        }

        public bool Validate(string password, out string errorMessage)
        {
            errorMessage = null;
            if (password.Contains(" "))
            {
                errorMessage = "A password is not allowed to contain whitespace.";
                return false;
            }
            if (password.Length < MinimumLength)
            {
                errorMessage = $"A password must be at least {MinimumLength} characters long.";
                return false;
            }
            return true;
        }
    }
}
