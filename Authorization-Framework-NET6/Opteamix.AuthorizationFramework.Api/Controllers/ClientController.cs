using Microsoft.AspNetCore.Mvc;
using Opteamix.AuthorizationFramework.Common.Interface;
using Opteamix.AuthorizationFramework.Data.Model;
using Opteamix.AuthorizationFramework.Database.DTO;
using Opteamix.AuthorizationFramework.Common;

namespace Opteamix.AuthorizationFramework.Api.Controllers
{
    [ApiController]
    [Route("api/")]
    public class ClientController : ControllerBase
    {
        private readonly IClientsBiz _clientsBiz;
        public  ClientController(IClientsBiz clientsBiz)
        {
            _clientsBiz = clientsBiz;
        }

        [HttpGet]
        [Route("clients")]
        public IActionResult GetClients()
        {
            var result = _clientsBiz.GetClients().ToList();

            if (result.Count == 0)
            {
                return NotFound(Constants.ResultNotFound);
            }
            else
            {
                return Ok(result);
            }
        }

        [HttpGet]
        [Route("client")]
        public IActionResult GetClient(int clientId)
        {
            var result = _clientsBiz.GetClient(clientId);

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
        [Route("client/add")]
        public IActionResult AddClient([FromBody] ClientDTO clientDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                Client client = new Client
                {
                    Name = clientDTO.Name,
                    ShortName = clientDTO.ShortName,
                    ClientStatus = 1,
                    IsDeleted = false,
                    ClientLogoImage = clientDTO.ClientLogoImage,
                    ClientLogoName = clientDTO.ClientLogoName,
                    ClientLogoImageType = clientDTO.ClientLogoImageType,
                    EmailAddress = clientDTO.EmailAddress,
                    WebsiteAddress = clientDTO.WebsiteAddress,
                    AddressLine1 = clientDTO.AddressLine1,
                    AddressLine2 = clientDTO.AddressLine2,
                    ContactPerson = clientDTO.ContactPerson,
                    ContactPersonEmailAddress = clientDTO.ContactPersonEmailAddress,
                    ContactPersonPhoneNumber = clientDTO.ContactPersonPhoneNumber,
                    City = clientDTO.City
                };

                var response = _clientsBiz.AddClient(client);

                return Ok(response);
            }
        }


        [HttpPost]
        [Route("client/delete")]
        public IActionResult DeleteClient(List<int> id)
        {
            var result = _clientsBiz.DeleteClient(id);

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
        [Route("clientsPagination")]
        public PageResult<FetchClientDTO> GetClientsPatination(int PageIndex, int PageSize)
        {
            var result = _clientsBiz.GetClients();
            
            if (result == null)
            {
                return new();
            }
            else
            {
                var totalRecord = result.Count();
                var skip = (PageIndex - 1) * PageSize;

                //if pagination not require
                if(PageSize == 0)
                {
                    PageSize = totalRecord;
                    PageIndex = 1;
                }

                var pages = (double)totalRecord / PageSize;
                var pageCount = (int)Math.Ceiling(pages);

                var selectedClient = new PageResult<FetchClientDTO>
                {
                    CurrentPage = PageIndex,
                    PageCount = pageCount,
                    PageSize = PageSize,
                    RowCount = totalRecord,
                    Result = result.Skip(skip).Take(PageSize),
                    
                };
                return selectedClient;
            }
        }
        [HttpPut]
        [Route("client/update")]
        public IActionResult UpdateClient([FromBody] UpdateClientDTO clientDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                Client client = new Client
                {
                    Id=clientDTO.ClientId,
                    Name = clientDTO.Name,
                    ShortName = clientDTO.ShortName,
                    ClientStatus = 1,
                    IsDeleted = false,
                    ClientLogoImage = clientDTO.ClientLogoImage,
                    ClientLogoName = clientDTO.ClientLogoName,
                    ClientLogoImageType = clientDTO.ClientLogoImageType,
                    EmailAddress = clientDTO.EmailAddress,
                    WebsiteAddress = clientDTO.WebsiteAddress,
                    AddressLine1 = clientDTO.AddressLine1,
                    AddressLine2 = clientDTO.AddressLine2,
                    ContactPerson = clientDTO.ContactPerson,
                    ContactPersonEmailAddress = clientDTO.ContactPersonEmailAddress,
                    ContactPersonPhoneNumber = clientDTO.ContactPersonPhoneNumber,
                    City = clientDTO.City
                };

                var response = _clientsBiz.UpdateClient(client);

                return Ok(response);
            }
        }

        
    }
}
