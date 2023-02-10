using System.ComponentModel.DataAnnotations;

namespace Opteamix.AuthorizationFramework.BlazorApp.Models.Modules
{
    public class AddModuleModel
    {
        public string Name { get; set; }

        public string Abbreviation { get; set; }

        public int? ParentModule { get; set; }

        public int? ApplicationId { get; set; }

        public string? Description { get; set; }

        public int? Id { get; set; }
    }
}
