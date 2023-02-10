using System.ComponentModel.DataAnnotations;

namespace Opteamix.AuthorizationFramework.BlazorApp.Models.Roles
{
    public class AddRoleModel
    {

        public string RoleName { get; set; }

        public string ShortName { get; set; }

        public string? RoleDescription { get; set; }

        public int ApplicationId { get; set; }

        public int Id { get; set; }
    }
}