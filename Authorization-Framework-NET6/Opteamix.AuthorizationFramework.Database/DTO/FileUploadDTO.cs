using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Opteamix.AuthorizationFramework.Database.DTO
{
    public class FileUploadDTO
    {
         public byte[] content { get; set; }
        public string fileName { get; set; }
        public int userId { get; set; }
        public string clientName { get; set; }
        public string applicationName { get; set; }
    }
}