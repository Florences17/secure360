using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opteamix.AuthorizationFramework.Database.DTO
{
    public class AddApplicationDTO
    {
        public string? Name { get; set; }
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Abbreviation { get; set; }
        public string? Description { get; set; }
        public byte[]? LogoImage { get; set; }
        public string? LogoImageName { get; set; }
        public string? LogoImageType { get; set; }
        public int ClientId { get; set; }
        public bool Active { get; set; }
    }
}
