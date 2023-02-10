using System.ComponentModel.DataAnnotations;

namespace Opteamix.AuthorizationFramework.BlazorApp.Models.Roles
{
    public class Role
    {

        public string RoleName { get; set; }

        public string ShortName { get; set; }

        public string? RoleDescription { get; set; }
    }
}