using Opteamix.AuthorizationFramework.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opteamix.AuthorizationFramework.Common.Interfaces
{
    public interface IAddRoleDataManager<T>
    {
        IEnumerable<Role> GetRole(int roleid);

        IEnumerable<Role> GetRoleid(Role role); 

        IEnumerable<EntityPrivilege> GetEntPrevId(EntityPrivilege entprev);

        string AddRole(Role role);

        string DeleteRole(List<int> roleid);

        Task<bool> UpdateRole(Role role);

        string AddModulePrivilege(EntityPrivilege entprev); 

        string AddRoleModule(RoleEntity roleEnt);

        IEnumerable<RoleEntity> GetRoleEntityByRoleId(int roleId);

        IEnumerable<EntityPrivilege> GetEntityPrivilegeById(int id);
        public int GetTopEntityValue();

    }
}
