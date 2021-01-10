namespace MicroIdentityService
{
    /// <summary>
    /// General constants of the MicroIdentityService.
    /// </summary>
    public static class MisConstants
    {
        // The MIS domain and administrator role
        public static readonly string MIS_DOMAIN_NAME = "mis";
        public static readonly string MIS_ADMINISTRATOR_ROLE_NAME = "admin";
        public static readonly string MIS_ADMINISTRATOR_ROLE = "mis.admin";

        // JWT keys for roles and permissions
        public static readonly string JWT_ROLES = "roles";
        public static readonly string JWT_PERMISSIONS = "permissions";
    }
}
