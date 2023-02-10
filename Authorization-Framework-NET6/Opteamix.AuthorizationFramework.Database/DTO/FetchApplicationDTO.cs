using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opteamix.AuthorizationFramework.Database.DTO
{
    public class FetchApplicationDTO
    {
        public int ApplicationId { get; set; }

        public string ApplicationName { get; set; }

        public string Abbreviation { get; set; }

        public string? Description { get; set; }

        public byte[]? LogoImage { get; set; }

        public string? LogoImageName { get; set; }

        public string? LogoImageType { get; set; }
        public string? Code { get; set; }
        public int ClientId { get; set; }
        public int ModifyBy { get; set; }
        public DateTime Created { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime ModifyDateTime { get; set; }

    }
}
