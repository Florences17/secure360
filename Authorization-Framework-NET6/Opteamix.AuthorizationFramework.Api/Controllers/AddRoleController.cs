using Microsoft.AspNetCore.Mvc;
using Opteamix.AuthorizationFramework.Common.Interfaces;
using Opteamix.AuthorizationFramework.Data.Model;
using Opteamix.AuthorizationFramework.Database.DTO;
using Opteamix.AuthorizationFramework.Common;


namespace Opteamix.AuthorizationFramework.Api.Controllers
{
    [ApiController]
    [Route("api/")]
    public class AddRoleController : ControllerBase
    {
        private readonly IAddRoleBiz _addroleBiz;

        public AddRoleController(IAddRoleBiz addroleBiz)
        {
            _addroleBiz = addroleBiz;
        }

        [HttpGet]
        [Route("Roles")]
        public IActionResult GetRole(int appid)
        {
            var result = _addroleBiz.GetRole(appid);

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
        [Route("RoleId/get")]
        public IActionResult GetRoleId([FromBody] AddRoleDTO RoleDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
               
                Role role = new Role
                {
                    RoleName = RoleDTO.RoleName,
                    ShortName = RoleDTO.ShortName,
                    ApplicationId = RoleDTO.ApplicationId,
                    RoleDescription = String.IsNullOrEmpty(RoleDTO.RoleDescription) ? null : RoleDTO.RoleDescription
                };
                var result = _addroleBiz.GetRoleId(role);
                return Ok(result);
            }
               
        }

        [HttpGet]
        [Route("RoleEntityByRoleId")]
        public IActionResult GetRoleEntityByRoleId(int roleId)
        {
                var result = _addroleBiz.GetRoleEntityByRoleId(roleId);
                return Ok(result);

        }

        [HttpGet]
        [Route("GetEntityPrivilegeById")]
        public IActionResult GetEntityPrivilegeById(int id)
        {
            var result = _addroleBiz.GetEntityPrivilegeById(id);
            return Ok(result);

        }

        [HttpPost]
        [Route("ModPrevId/get")]
        public IActionResult ModPrevId([FromBody] ModulePrivilegeDTO ModPrivDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {

                EntityPrivilege entprev = new EntityPrivilege
                {
                    ModuleId = ModPrivDTO.Moduleid,
                    PrivilegeId = ModPrivDTO.Privilegeid
                };
                var result = _addroleBiz.GetEntPrevId(entprev);
                return Ok(result);
            }

        }

        [HttpPost]
        [Route("Roles/add")]
        public IActionResult AddRoles([FromBody] AddRoleDTO RoleDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                Role role = new Role
                {
                    RoleName = RoleDTO.RoleName,
                    ShortName = RoleDTO.ShortName,
                    ApplicationId = RoleDTO.ApplicationId,
                    RoleDescription = String.IsNullOrEmpty(RoleDTO.RoleDescription) ? null : RoleDTO.RoleDescription
                };

                var response = _addroleBiz.AddRole(role);

                return Ok(response);
            }
        }

        [HttpPost]
        [Route("Roles/addModPriv")]
        public IActionResult AddModulePrivilege([FromBody] ModulePrivilegeDTO ModPrivDTO)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                EntityPrivilege EntPrev = new EntityPrivilege
                {
                    ModuleId = ModPrivDTO.Moduleid,
                    PrivilegeId = ModPrivDTO.Privilegeid,
                    PermissionId = ModPrivDTO.PermissionId == 0 ? null : ModPrivDTO.PermissionId
                };
               
                _addroleBiz.AddModulePrivilege(EntPrev);
                var id=EntPrev.Id;
                RoleEntity roleEnt = new RoleEntity
                {
                    PermissionprivilegeId = id,
                    RoleId = ModPrivDTO.roleid,
                    CreatedDate= DateTime.Now,  
                    ModifiedDate= DateTime.Now,

                };

                var response = _addroleBiz.AddRoleModule(roleEnt);

                return Ok(response);
            }
        }

        //public IActionResult AddModulePrivilege([FromBody] ModulePrivilegeDTO ModPrivDTO)
        //{

        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest();
        //    }
        //    else
        //    {
        //        EntityPrivilege EntPrev = new EntityPrivilege
        //        {
        //            ModuleId = ModPrivDTO.Moduleid,
        //            PrivilegeId = ModPrivDTO.Privilegeid
        //        };

        //        _addroleBiz.AddModulePrivilege(EntPrev);
        //        var id = EntPrev.Id;
        //        RoleEntity roleEnt = new RoleEntity
        //        {
        //            PermissionprivilegeId = id,
        //            RoleId = ModPrivDTO.roleid
        //        };

        //        var response = _addroleBiz.AddRoleModule(roleEnt);

        //        return Ok(response);
        //    }
        //}

        [HttpPost]
        [Route("roles/addRoleModule")]
        public IActionResult AddRoleModule([FromBody] RoleModuleDTO roleModuleDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                RoleEntity roleEnt = new RoleEntity
                {
                    PermissionprivilegeId = roleModuleDTO.ModulePrevilegeId,
                    RoleId = roleModuleDTO.roleId
                };

                var response = _addroleBiz.AddRoleModule(roleEnt);

                return Ok(response);
            }
        }

        [HttpPost]
        [Route("Roles/delete")]
        public IActionResult DeleteApplication(List<int> roleId)
        {
            var result = _addroleBiz.DeleteRole(roleId);

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
        [Route("Roles/update")]
        public IActionResult UpdateRole([FromBody] AddRoleDTO roleDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                Role roleDetail = new Role
                {
                    RoleName = roleDTO.RoleName,
                    ShortName = roleDTO.ShortName,
                    RoleDescription = roleDTO.RoleDescription,
                    Id = roleDTO.Id
                };

                var response = _addroleBiz.UpdateRole(roleDetail);

                return Ok(response);
            }
        }

    }
}
