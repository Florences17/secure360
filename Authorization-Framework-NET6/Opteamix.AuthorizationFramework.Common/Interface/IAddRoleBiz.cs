using Opteamix.AuthorizationFramework.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opteamix.AuthorizationFramework.Common.Interfaces
{
    public interface IAddRoleBiz
    {
        IEnumerable<Role> GetRole(int roleid);

        string AddRole(Role role);
        string AddModulePrivilege(EntityPrivilege entprev);

        string AddRoleModule(RoleEntity roleEnt);
        
        IEnumerable<Role> GetRoleId(Role role);

        IEnumerable<EntityPrivilege> GetEntPrevId(EntityPrivilege entprev);

        string DeleteRole(List<int> roleid);

        Task<bool> UpdateRole(Role client);

        IEnumerable<RoleEntity> GetRoleEntityByRoleId(int roleId);

        IEnumerable<EntityPrivilege> GetEntityPrivilegeById(int id);
        public int GetTopEntityValue();
    }
}