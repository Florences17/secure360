using Opteamix.AuthorizationFramework.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Opteamix.AuthorizationFramework.Database.DTO;


namespace Opteamix.AuthorizationFramework.Common.Interfaces
{
    public interface IPrivilegeBiz
    {
        IEnumerable<FetchPrivilegeDTO> GetPrivilege(int privilegeid);

         IEnumerable<FetchPrivilegeDTO> GetPrivileges(int appId);
          string AddPrivilege(Privilege privilege);
          
        Task<bool> UpdatePrivilege(Privilege privilege);

        string DeletePrivilege(List<int> privilegeid);
    }
}
