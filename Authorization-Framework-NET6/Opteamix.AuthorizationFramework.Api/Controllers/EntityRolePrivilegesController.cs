using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Opteamix.AuthorizationFramework.Common.Interfaces;
using Opteamix.AuthorizationFramework.Data.Model;
using Opteamix.AuthorizationFramework.Database.DTO;
using Opteamix.AuthorizationFramework.Common;
namespace Opteamix.AuthorizationFramework.Api.Controllers
{
    [ApiController]
    [Route("api/")]
    public class EntityRolePrivilegesController : ControllerBase
    {
        private readonly IModulesBiz _moduleBiz;
        public EntityRolePrivilegesController(IModulesBiz modulesBiz)
        {
            _moduleBiz = modulesBiz;
        }
        [HttpGet]
        [Route("GetEntityRolePrivileges")]
        public IActionResult GetEntityRolePrivileges(string clientName, string applicationName)
        {
            var result = _moduleBiz.GetEntityRolePrivileges(clientName, applicationName);
            if (result == null)
            {
                return NotFound(Constants.ResultNotFound);
            }
            else
            {
                return Ok(result);
            }
        }
        [HttpGet]
        [Route("GetEntityRolePrivileges1")]
        public IActionResult GetEntityRolePrivileges1(string clientName, string applicationName)
        {
            var result = _moduleBiz.GetEntityRolePrivileges(clientName, applicationName);
            if (result == null)
            {
                return NotFound(Constants.ResultNotFound);
            }
            else
            {
                return Ok(result);
            }
        }
    }
}
   
