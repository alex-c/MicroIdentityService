namespace MicroIdentityService.Services.PasswordValidation.Validators
{
    /// <summary>
    /// A validator that always successfully validates regardless of input.
    /// </summary>
    public class NoOpPasswordValidator : IPasswordValidator
    {
        public bool Validate(string password, out string errorMessage)
        {
            errorMessage = null;
            return true;
        }
    }
}
