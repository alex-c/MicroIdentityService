namespace MicroIdentityService.Authorization
{
    /// <summary>
    /// Holds the names of authorization policies.
    /// </summary>
    public static class Policies
    {
        public const string API_KEYS_GET = "api-keys.get";
        public const string API_KEYS_CREATE = "api-keys.create";
        public const string API_KEYS_UPDATE = "api-keys.update";
        public const string API_KEYS_DELETE = "api-keys.delete";
        public const string API_KEYS_GET_PERMISSIONS = "api-keys.get-permissions";
        public const string API_KEYS_SET_PERMISSIONS = "api-keys.set-permissions";

        public const string DOMAINS_GET = "domains.get";
        public const string DOMAINS_CREATE = "domains.create";
        public const string DOMAINS_UPDATE = "domains.update";
        public const string DOMAINS_DELETE = "domains.delete";

        public const string IDENTITIES_GET = "identities.get";
        public const string IDENTITIES_CREATE = "identities.create";
        public const string IDENTITIES_UPDATE = "identities.update";
        public const string IDENTITIES_DELETE = "identities.delete";
        public const string IDENTITIES_GET_ROLES = "identities.get-roles";
        public const string IDENTITIES_SET_ROLES = "identities.set-roles";

        public const string ROLES_GET = "roles.get";
        public const string ROLES_CREATE = "roles.create";
        public const string ROLES_UPDATE = "roles.update";
        public const string ROLES_DELETE = "roles.delete";
    }
}
