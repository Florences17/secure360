using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opteamix.AuthorizationFramework.Database.DTO
{
    public class FetchUserDTO
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName { get; set; }

        public string UserName { get; set; }

        public string? Email { get; set; }

        public int? RoleId { get; set; }

        public string? Role { get; set; }

        public int? ManagerId { get; set; }

        public string? ManagerName { get; set; }

        public string? Status { get; set; }
    }
}
