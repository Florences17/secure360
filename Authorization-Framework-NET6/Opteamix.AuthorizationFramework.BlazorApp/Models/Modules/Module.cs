using Opteamix.AuthorizationFramework.BlazorApp.Models.Privilege;

namespace Opteamix.AuthorizationFramework.BlazorApp.Models.Modules
{
    public class Module
    {
        public int ModuleId { get; set; }

        public string ModuleName { get; set; }

        public string? Abbreviation { get; set; }

        public string? Description { get; set; }

        public string ApplicationName { get; set; }

        public int? ModuleType { get; set; }

        public List<FetchPrivilegeModel> fetchPrivileges { get; set; }
    }
}
