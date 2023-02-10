using Opteamix.AuthorizationFramework.Data.Model;
using Opteamix.AuthorizationFramework.Database.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opteamix.AuthorizationFramework.Common.Interfaces
{
    public interface IModulesDataManager<T>
    {
        IEnumerable<FetchModuleDTO> GetModules(int appid);

        IEnumerable<FetchModuleDTO> GetModule(int moduleid);

        IEnumerable<FetchSubModuleDTO> GetSubModules();

        IEnumerable<FetchSubModuleDTO> GetSubModules(int moduleid);

        IEnumerable<FetchSubModuleDTO> GetL2SubModules(int submoduleid);

        IEnumerable<FetchSubModuleDTO> GetSubModule(int submoduleid);

        string AddModule(Module module);

        string DeleteModule(List<int> moduleid);

        string AddSubModule(Entity entity);

        Task<bool> UpdateModule(Module module);
        IEnumerable<EntityRolePrivilege> GetEntityRolePrivileges(string clientName, string applicationName);
    }
}
