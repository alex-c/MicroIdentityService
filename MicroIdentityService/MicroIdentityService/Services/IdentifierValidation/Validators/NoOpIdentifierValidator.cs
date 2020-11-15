namespace MicroIdentityService.Services.IdentifierValidation.Validators
{
    /// <summary>
    /// A validator that always successfully validates regardless of input.
    /// </summary>
    public class NoOpIdentifierValidator : IIdentifierValidator
    {
        public bool Validate(string identifier, out string errorMessage)
        {
            errorMessage = null;
            return true;
        }
    }
}
