using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office2010.Excel;
using Opteamix.AuthorizationFramework.Common.Interfaces;
using Opteamix.AuthorizationFramework.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opteamix.AuthorizationFramework.Common.BusinessComponent
{
    public class AddRoleBiz : IAddRoleBiz
    {
        IAddRoleDataManager<Role> RoleRepo;

        public AddRoleBiz(IAddRoleDataManager<Role> repo)
        {
            RoleRepo = repo;
        }

        public IEnumerable<Role> GetRole(int roleid)
        {
            return RoleRepo.GetRole(roleid);
        }

        public IEnumerable<Role> GetRoleId(Role role)
        {
            return RoleRepo.GetRoleid(role);
        }

        public IEnumerable<EntityPrivilege> GetEntPrevId(EntityPrivilege entprev)
        {
            return RoleRepo.GetEntPrevId(entprev);
        }

        public string AddRole(Role role)
        {
            return RoleRepo.AddRole(role);
        }

        public string DeleteRole(List<int> roleid)
        {
            return RoleRepo.DeleteRole(roleid);
        }

        public async Task<bool> UpdateRole(Role role)
        {
            return await RoleRepo.UpdateRole(role);
        }

        public string AddModulePrivilege(EntityPrivilege entprev)
        {
            return RoleRepo.AddModulePrivilege(entprev);
        }

        public string AddRoleModule(RoleEntity roleEnt)
        {
            return RoleRepo.AddRoleModule(roleEnt);
        }

        public IEnumerable<RoleEntity> GetRoleEntityByRoleId(int roleId)
        {
            return RoleRepo.GetRoleEntityByRoleId(roleId);
        }

        public IEnumerable<EntityPrivilege> GetEntityPrivilegeById(int id)
        {
            return RoleRepo.GetEntityPrivilegeById(id);
        }
        public int GetTopEntityValue()
        {
            return RoleRepo.GetTopEntityValue();
        }
    }
}
