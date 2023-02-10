using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opteamix.AuthorizationFramework.Database.DTO
{
    public class AddRoleDTO
    {
        public string RoleName { get; set; }

        public string ShortName { get; set; }

        public string? RoleDescription { get; set; }

        public int ApplicationId { get; set; }

        public int Id { get; set; }
    }
}
