namespace Opteamix.AuthorizationFramework.BlazorApp.Models.Modules
{
    public class SubModule
    {
        public int SubModuleId { get; set; }

        public string SubModuleName { get; set; }

        public string? Abbreviation { get; set; }

        public int? ModuleId { get; set; }

        public int? ParentModuleId { get; set; }
    }
}
