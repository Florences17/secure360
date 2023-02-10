using Microsoft.AspNetCore.Mvc;
using Opteamix.AuthorizationFramework.Common.Interfaces;
using Opteamix.AuthorizationFramework.Data.Model;
using Opteamix.AuthorizationFramework.Database.DTO;
using Opteamix.AuthorizationFramework.Common;

namespace Opteamix.AuthorizationFramework.Api.Controllers
{
    [ApiController]
    [Route("api/")]
    public class PrivilegeController : ControllerBase
    {
        private readonly IPrivilegeBiz _privilegeBiz;

        public PrivilegeController(IPrivilegeBiz privilegeBiz)
        {
            _privilegeBiz = privilegeBiz;
        }

        [HttpGet]
        [Route("privileges")]
        public IActionResult GetPrivileges(int appId)
        {
            var result = _privilegeBiz.GetPrivileges(appId);

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
        [Route("privilege")]
        public IActionResult GetPrivilege(int privilegeId)
        {
            var result = _privilegeBiz.GetPrivilege(privilegeId);

            if (result == null)
            {
                return NotFound(Constants.ResultNotFound);
            }
            else
            {
                return Ok(result);
            }
        }

        [HttpPost]
        [Route("privilege/add")]
        public IActionResult AddPrivilege([FromBody] AddPrivilegeDTO privilegeDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                Privilege privilege = new Privilege
                {
                    PrivilegeName = privilegeDTO.PrivilegeName,
                    Description = privilegeDTO.Description,
                    AppId = privilegeDTO.ApplicationId
                };

                var response = _privilegeBiz.AddPrivilege(privilege);

                return Ok(response);
            }
        }

        [HttpPost]
        [Route("privileges/delete")]
        public IActionResult DeletePrivilege(List<int> id)
        {
            var result = _privilegeBiz.DeletePrivilege(id);

            if (result == null)
            {
                return NotFound(Constants.ResultNotFound);
            }
            else
            {
                return Ok(result);
            }
        }

        
        [HttpPut]
        [Route("privileges/update")]
        public IActionResult UpdatePrivilege([FromBody] AddPrivilegeDTO privilegeDTO) 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                Privilege privilege = new Privilege
                {
                    Id = (int)(String.IsNullOrEmpty(privilegeDTO.Id.ToString()) ? null : privilegeDTO.Id),
                    PrivilegeName = privilegeDTO.PrivilegeName,
                    Description = String.IsNullOrEmpty(privilegeDTO.Description) ? null : privilegeDTO.Description
                };

                var response = _privilegeBiz.UpdatePrivilege(privilege);

                return Ok(response);
            }
        }

    }
}
