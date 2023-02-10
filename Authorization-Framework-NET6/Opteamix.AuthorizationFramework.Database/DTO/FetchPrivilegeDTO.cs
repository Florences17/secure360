using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opteamix.AuthorizationFramework.Database.DTO
{
    public class FetchPrivilegeDTO
    {
        public int PrivilegeId { get; set; }

        public string PrivilegeName { get; set; }

        public string? Description { get; set; }


    }
}
