using Opteamix.AuthorizationFramework.Common.Interfaces;
using Opteamix.AuthorizationFramework.Data.Model;
using Opteamix.AuthorizationFramework.Database.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opteamix.AuthorizationFramework.Common.BusinessComponent
{
    public class ModulesBiz : IModulesBiz
    {
        IModulesDataManager<Module> ModuleRepo;

        public ModulesBiz(IModulesDataManager<Module> repo)
        {
            ModuleRepo = repo;
        }

        public IEnumerable<FetchModuleDTO> GetModules(int appid)
        {
            return ModuleRepo.GetModules(appid);
        }

        public IEnumerable<FetchModuleDTO> GetModule(int moduleid)
        {
            return ModuleRepo.GetModule(moduleid);
        }

        public IEnumerable<FetchSubModuleDTO> GetSubModules()
        {
            return ModuleRepo.GetSubModules();
        }
        public IEnumerable<EntityRolePrivilege> GetEntityRolePrivileges(string clientName, string applicationName)
        {
            return ModuleRepo.GetEntityRolePrivileges(clientName, applicationName);
        }

        public IEnumerable<FetchSubModuleDTO> GetSubModules(int moduleid)
        {
            return ModuleRepo.GetSubModules(moduleid);
        }

        public IEnumerable<FetchSubModuleDTO> GetL2SubModules(int submoduleid)
        {
            return ModuleRepo.GetL2SubModules(submoduleid);
        }

        public IEnumerable<FetchSubModuleDTO> GetSubModule(int submoduleid)
        {
            return ModuleRepo.GetSubModule(submoduleid);
        }

        public string AddModule(Module module)
        {
            return ModuleRepo.AddModule(module);
        }

        public string AddSubModule(Entity entity)
        {
            return ModuleRepo.AddSubModule(entity);
        }

        public async Task<bool> UpdateModule(Module module)
        {
            return await ModuleRepo.UpdateModule(module);
        }

        public string DeleteModule(List<int> moduleid)
        {
            return ModuleRepo.DeleteModule(moduleid);
        }
    }
}
