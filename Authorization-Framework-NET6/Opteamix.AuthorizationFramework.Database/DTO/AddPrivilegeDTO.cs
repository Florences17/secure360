using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opteamix.AuthorizationFramework.Database.DTO
{
    public class AddPrivilegeDTO
    {     public int? Id { get; set; }
        public string? PrivilegeName { get; set; }
       
        public int ApplicationId { get; set; }

        public string? Description { get; set; }

    }
}
