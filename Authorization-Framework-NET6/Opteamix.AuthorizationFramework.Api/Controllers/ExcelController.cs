using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Opteamix.AuthorizationFramework.Common.Interface;
using Opteamix.AuthorizationFramework.Database.DTO;

namespace Opteamix.AuthorizationFramework.Api.Controllers
{
    [Route("api/[controller]")]
    public class ExcelController : Controller
    { 
        private readonly IExcelBusiness _excelBc;      

        public ExcelController(IExcelBusiness excelBc)
        {
            _excelBc = excelBc;
        }

        [HttpPost("Import")]       
        public async Task<IActionResult> ImportExcel(string ClientId, string ApplicationId, IFormFile file, int? userId = null)
        {
            try
            {
                userId = int.Parse(ClientId);
                await _excelBc.ImportExcel(ClientId, ApplicationId, file, userId);
            }
            catch (Exception)
            {
                throw;
            }
            return Ok("Excel imported successfully");
           
        }

        [HttpGet("Export")]       
        public async Task<IActionResult> ExportExcel(string ClientId, string ApplicationId)
        {
           var memory =  await _excelBc.ExportExcel(ClientId, ApplicationId);
           return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet","ExportExcel.xlsx");
        }
    }
}