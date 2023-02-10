using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opteamix.AuthorizationFramework.Database.DTO
{
    public class AddModuleDTO
    {
        public int? Id { get; set; }

        public string Name { get; set; }

        public string Abbreviation { get; set; }

        public int ApplicationId { get; set; }

        public string? Description { get; set; }

        public int? ParentModule { get; set; }

    }
}
