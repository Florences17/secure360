using System.ComponentModel.DataAnnotations;

namespace Opteamix.AuthorizationFramework.BlazorApp.Models.Privilege
{
    public class AddPrivilegeModel
    {

        public string PrivilegeName { get; set; }

        public string? Description { get; set; }

        public int ApplicationId { get; set; }
        
        public int? Id { get; set; }
    }
}
