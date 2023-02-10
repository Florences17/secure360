using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Opteamix.AuthorizationFramework.Common.Interface
{
    public interface IExcelBusiness
    {
        Task<MemoryStream> ExportExcel(string clientName, string applicationName);
        Task<bool> ImportExcel(string clientName, string applicationName, IFormFile file, int? userId = null);
    }
}