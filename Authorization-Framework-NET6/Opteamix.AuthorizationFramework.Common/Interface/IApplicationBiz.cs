using Opteamix.AuthorizationFramework.Data.Model;
using Opteamix.AuthorizationFramework.Database.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opteamix.AuthorizationFramework.Common.Interface
{
    public interface IApplicationBiz
    {
        IEnumerable<FetchApplicationDTO> GetApplications(int clientid);
         IEnumerable<FetchApplicationDTO> GetAllApplications();
        Task<bool> AddApplication(Application aapplication);
        Task<bool> DeleteApplication(int Appid);
        IEnumerable<FetchApplicationDTO> GetApplication(int Appid);
        Task<bool> UpdateApplication(Application aapplication);
    }
}
