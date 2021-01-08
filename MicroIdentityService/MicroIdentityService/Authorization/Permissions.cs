namespace MicroIdentityService.Authorization
{
    /// <summary>
    /// Holds the names of fine-grained endpoint permissions.
    /// </summary>
    public static class Permissions
    {
        public const string API_KEYS_GET = "api-keys.get";
        public const string API_KEYS_CREATE = "api-keys.create";
        public const string API_KEYS_UPDATE = "api-keys.update";
        public const string API_KEYS_DELETE = "api-keys.delete";

        public const string DOMAINS_GET = "domains.get";
        public const string DOMAINS_CREATE = "domains.create";
        public const string DOMAINS_UPDATE = "domains.update";
        public const string DOMAINS_DELETE = "domains.delete";

        public const string USERS_GET = "users.get";
        public const string USERS_CREATE = "users.create";
        public const string USERS_UPDATE = "users.update";
        public const string USERS_DELETE = "users.delete";
        public const string USERS_GET_ROLES = "users.get-roles";
        public const string USERS_SET_ROLES = "users.set-roles";

        public const string ROLES_GET = "roles.get";
        public const string ROLES_CREATE = "roles.create";
        public const string ROLES_UPDATE = "roles.update";
        public const string ROLES_DELETE = "roles.delete";
    }
}
