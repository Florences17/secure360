using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Opteamix.AuthorizationFramework.Common.Interfaces;
using Opteamix.AuthorizationFramework.Data.Model;
using Opteamix.AuthorizationFramework.Database.DTO;


namespace Opteamix.AuthorizationFramework.Common.BusinessComponent
{
 public class PrivilegeBiz: IPrivilegeBiz
    {
        IPrivilegeDataManager<Privilege> PrivilegeRepo;
        public PrivilegeBiz(IPrivilegeDataManager<Privilege> repo) => PrivilegeRepo = repo;

        public IEnumerable<FetchPrivilegeDTO> GetPrivileges(int appId)
        {
            return PrivilegeRepo.GetPrivileges(appId);
        }
        public IEnumerable<FetchPrivilegeDTO> GetPrivilege(int privilegeid)
        {
            return  PrivilegeRepo.GetPrivilege(privilegeid);
        }
         public string AddPrivilege(Privilege privilege)
        {
            return PrivilegeRepo.AddPrivilege(privilege);
        }
        
        public async Task<bool> UpdatePrivilege(Privilege privilege)
        {
            return await PrivilegeRepo.UpdatePrivilege(privilege);
        }

         public string DeletePrivilege(List<int> privilegeid)
        {
            return PrivilegeRepo.DeletePrivilege(privilegeid);
        }
    }


}
