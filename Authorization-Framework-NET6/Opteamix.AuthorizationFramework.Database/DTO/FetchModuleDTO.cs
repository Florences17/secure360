using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opteamix.AuthorizationFramework.Database.DTO
{
    public class FetchModuleDTO
    {
        public int ModuleId { get; set; }

        public string ModuleName { get; set; }

        public string? Abbreviation { get; set; }

        public string? Description { get; set; }

        public string ApplicationName { get; set; }

    }
}
