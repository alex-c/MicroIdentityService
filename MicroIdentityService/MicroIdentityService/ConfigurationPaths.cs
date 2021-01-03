namespace MicroIdentityService
{
    public static class ConfigurationPaths
    {
        public static string PERSISTENCE_STRATEGY = "Persistence:Strategy";
        public static string DATABASE_CONNECTION_STRING = "Database:ConnectionString";

        public static string CREATE_ADMINISTRATOR_IF_MISSING = "Administrator:CreateIfMissing";
        public static string ADMINISTRATOR_IDENTIFIER = "Administrator:Identifier";
        public static string ADMINISTRATOR_PASSWORD = "Administrator:Password";

        public static string JWT_SECRET = "Jwt:Secret";
        public static string JWT_ISSUER = "Jwt:Issuer";
        public static string JWT_LIFETIME_IN_MINUTES = "Jwt:LifetimeInMinutes";

        public static string IDENTIFIER_VALIDATION_STRATEGY = "IdentifierValidation:Strategy";
        public static string PASSWORD_VALIDATION_STRATEGY = "PasswordValidation:Strategy";


        public static string PASSWORD_VALIDATION_MINIMUM_LENGTH = "PasswordValidation:Configuration:MinimumLength";
    }
}
