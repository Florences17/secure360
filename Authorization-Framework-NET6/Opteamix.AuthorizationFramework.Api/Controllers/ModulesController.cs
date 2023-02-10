using Microsoft.AspNetCore.Mvc;
using Opteamix.AuthorizationFramework.Common.Interfaces;
using Opteamix.AuthorizationFramework.Data.Model;
using Opteamix.AuthorizationFramework.Database.DTO;
using Opteamix.AuthorizationFramework.Common;

namespace Opteamix.AuthorizationFramework.Api.Controllers
{
    [ApiController]
    [Route("api/")]
    public class ModulesController : ControllerBase
    {
        private readonly IModulesBiz _moduleBiz;

        public ModulesController(IModulesBiz modulesBiz)
        {
            _moduleBiz = modulesBiz;
        }

        [HttpGet]
        [Route("modules")]
        public IActionResult GetModules(int appid)
        {
            var result = _moduleBiz.GetModules(appid);

            if (result == null)
            {
                return NotFound(Constants.ResultNotFound);
            }
            else
            {
                //var data = result.Select(m => new { ModuleId = m.Id });
                return Ok(result);
            }
        }

        [HttpGet]
        [Route("module")]
        public IActionResult GetModule(int moduleid)
        {
            var result = _moduleBiz.GetModule(moduleid);

            if (result == null)
            {
                return NotFound(Constants.ResultNotFound);
            }
            else
            {
                //var data = result.Select(m => new { ModuleId = m.Id });
                return Ok(result);
            }
        }

        [HttpGet]
        [Route("allsubmodules")]
        public IActionResult GetSubModules()
        {
            var result = _moduleBiz.GetSubModules();

            if (result == null)
            {
                return NotFound(Constants.ResultNotFound);
            }
            else
            {
                //var data = result.Select(m => new { ModuleId = m.Id });
                return Ok(result);
            }
        }

        [HttpGet]
        [Route("submodules")]
        public IActionResult GetSubModules(int moduleid)
        {
            var result = _moduleBiz.GetSubModules(moduleid);

            if (result == null)
            {
                return NotFound(Constants.ResultNotFound);
            }
            else
            {
                //var data = result.Select(m => new { ModuleId = m.Id });
                return Ok(result);
            }
        }

        [HttpGet]
        [Route("l2submodules")]
        public IActionResult GetL2SubModules(int submoduleid)
        {
            var result = _moduleBiz.GetL2SubModules(submoduleid);

            if (result == null)
            {
                return NotFound(Constants.ResultNotFound);
            }
            else
            {
                //var data = result.Select(m => new { ModuleId = m.Id });
                return Ok(result);
            }
        }

        [HttpGet]
        [Route("submodule")]
        public IActionResult GetSubModule(int submoduleid)
        {
            var result = _moduleBiz.GetSubModule(submoduleid);

            if (result == null)
            {
                return NotFound(Constants.ResultNotFound);
            }
            else
            {
                //var data = result.Select(m => new { ModuleId = m.Id });
                return Ok(result);
            }
        }

        [HttpPost]
        [Route("modules/add")]
        public IActionResult AddModule([FromBody] AddModuleDTO moduleDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                if (moduleDTO.ParentModule == 0)
                {
                    Module module = new Module
                    {
                        Name = moduleDTO.Name,
                        Abbreviation = moduleDTO.Abbreviation,
                        Description = String.IsNullOrEmpty(moduleDTO.Description) ? null : moduleDTO.Description,
                        ApplicationId = moduleDTO.ApplicationId,
                    };
                    var response = _moduleBiz.AddModule(module);
                    return Ok(response);
                }
                else 
                {
                    Entity entity = new Entity
                    {
                        DisplayName = moduleDTO.Name,
                        ModuleId = moduleDTO.ParentModule,
                        Abbreviation = moduleDTO.Abbreviation
                    };
                    var response = _moduleBiz.AddSubModule(entity);
                    return Ok(response);
                }  
            }
        }

        [HttpPut]
        [Route("modules/update")]
        public IActionResult UpdateModule([FromBody] AddModuleDTO moduleDTO) 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                Module module = new Module
                {
                    Id = (int)(String.IsNullOrEmpty(moduleDTO.Id.ToString()) ? null : moduleDTO.Id),
                    Name = moduleDTO.Name,
                    Abbreviation = moduleDTO.Abbreviation,
                    Description = String.IsNullOrEmpty(moduleDTO.Description) ? null : moduleDTO.Description,
                    ApplicationId = moduleDTO.ApplicationId
                };

                var response = _moduleBiz.UpdateModule(module);

                return Ok(response);
            }
        }
        
        [HttpPost]
        [Route("modules/delete")]
        public IActionResult DeleteApplication(List<int> moduleid)
        {
            var result = _moduleBiz.DeleteModule(moduleid);

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
