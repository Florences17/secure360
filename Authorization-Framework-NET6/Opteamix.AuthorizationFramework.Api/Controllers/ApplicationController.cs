using Microsoft.AspNetCore.Mvc;
using Opteamix.AuthorizationFramework.Common.Interface;
using Opteamix.AuthorizationFramework.Data.Model;
using Opteamix.AuthorizationFramework.Database.DTO;
using Opteamix.AuthorizationFramework.Common;

namespace Opteamix.AuthorizationFramework.Api.Controllers
{
    [ApiController]
    [Route("api/")]
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationBiz _applicationBiz;
        public ApplicationController(IApplicationBiz applicationBiz)
        {
            _applicationBiz = applicationBiz;
        }

        [HttpGet]
        [Route("application")]
        public IActionResult GetApplications(int clientid)
        {
            var result = _applicationBiz.GetApplications(clientid);

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
        [Route("applications")]
        public IActionResult GetAllApplications()
        {
            var result = _applicationBiz.GetAllApplications();

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
        [Route("application/add")]
        public IActionResult AddApplication([FromBody] AddApplicationDTO applicationDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                Application application = new Application
                {
                    Name = applicationDTO.Name,
                    Code = applicationDTO.Code,
                    Description = applicationDTO.Description,
                    Abbreviation=applicationDTO.Abbreviation,
                    LogoImage=applicationDTO.LogoImage,
                    LogoImageName=applicationDTO.LogoImageName,
                    LogoImageType=applicationDTO.LogoImageType,
                    ClientId=applicationDTO.ClientId,
                    ModifiedBy= applicationDTO.ClientId,
                    CreatedBy= applicationDTO.ClientId,
                    Active = true
                };

                var response = _applicationBiz.AddApplication(application);

                return Ok(response);
            }
        }

        [HttpPost]
        [Route("application/delete")]
        public IActionResult DeleteApplication(int applicationid)
        {
            var result = _applicationBiz.DeleteApplication(Convert.ToInt32(applicationid));

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
        [Route("application/getApplicationData")]
        public IActionResult GetApplicationData(int applicationId)
        {
            var result = _applicationBiz.GetApplication(applicationId);
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
        [Route("application/update")]
        public IActionResult UpdateApplication([FromBody] AddApplicationDTO applicationDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                Application application = new Application
                {
                    Id = applicationDTO.Id,
                    Name = applicationDTO?.Name,
                    Code = applicationDTO?.Code,
                    Description = applicationDTO?.Description,
                    Abbreviation = applicationDTO.Abbreviation,
                    LogoImage = applicationDTO?.LogoImage,
                    LogoImageName = applicationDTO?.LogoImageName,
                    LogoImageType = applicationDTO?.LogoImageType,
                    ClientId = applicationDTO.ClientId,
                    Active = true,
                    IsDeleted=false
                    
                };

                var response = _applicationBiz.UpdateApplication(application);

                return Ok(response);
            }
        }
    }
}
