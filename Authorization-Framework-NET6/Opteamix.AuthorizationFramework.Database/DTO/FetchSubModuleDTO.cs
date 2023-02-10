using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opteamix.AuthorizationFramework.Database.DTO
{
    public class FetchSubModuleDTO
    {
        public int SubModuleId { get; set; }

        public string SubModuleName { get; set; }

        public string? Abbreviation { get; set; }

        public int? ModuleId { get; set; }

        public int? ParentModuleId { get; set; }
    }
}
