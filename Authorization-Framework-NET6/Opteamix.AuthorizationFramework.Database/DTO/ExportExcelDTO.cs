using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opteamix.AuthorizationFramework.Database.DTO
{
    public class ExportExcelDTO
    {
        public int ClientId { get; set; }

        public int ApplicationId { get; set; }
    }
}
