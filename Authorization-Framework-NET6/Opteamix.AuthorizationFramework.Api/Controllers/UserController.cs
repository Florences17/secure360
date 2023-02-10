using Microsoft.AspNetCore.Mvc;
using Opteamix.AuthorizationFramework.Common;
using Opteamix.AuthorizationFramework.Common.Interface;

namespace Opteamix.AuthorizationFramework.Api.Controllers
{
    [ApiController]
    [Route("api/")]
    public class UserController : ControllerBase
    {
        private readonly IUserBiz _userBiz;

        public UserController(IUserBiz userBiz)
        {
            _userBiz = userBiz;
        }

        [HttpGet]
        [Route("users")]
        public IActionResult GetUsers(int clientid)
        {
            var result = _userBiz.GetUsers(clientid);

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
