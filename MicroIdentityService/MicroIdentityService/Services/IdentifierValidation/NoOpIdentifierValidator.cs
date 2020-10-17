namespace MicroIdentityService.Services.IdentifierValidation
{
    /// <summary>
    /// A validator that always successfully validates regardless of input.
    /// </summary>
    public class NoOpIdentifierValidator : IIdentifierValidator
    {
        public bool Validate(string identifier)
        {
            return true;
        }
    }
}
