using Opteamix.AuthorizationFramework.Data.Model;
using Opteamix.AuthorizationFramework.Database.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opteamix.AuthorizationFramework.Common.Interfaces
{
    public interface IPrivilegeDataManager<T>
    {
         IEnumerable<FetchPrivilegeDTO> GetPrivilege(int privilegeid);

        IEnumerable<FetchPrivilegeDTO> GetPrivileges(int appId);
        string AddPrivilege(Privilege privilege);

        string DeletePrivilege(List<int> privilegeid);
        Task<bool> UpdatePrivilege(Privilege privilege);
    }
}
