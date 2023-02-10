using Microsoft.EntityFrameworkCore;

namespace Opteamix.AuthorizationFramework.Data.Model
{
    [Keyless]
    public class EntityRolePrivilege
    {
        public string ClientName { get;  set; }
        public string ApplicationName { get;  set; }
        public string ? RoleName { get;  set; }
        public string Access { get;  set; }
        public string ClientShortName { get; set; }
        public string ApplicationShortName { get; set; }
        public string EntityName { get; set; }
        public string RoleShortName { get; set; }
        public string EntityShortName { get; set; }
        public string PrivilegeName { get; set; }

    }
}
