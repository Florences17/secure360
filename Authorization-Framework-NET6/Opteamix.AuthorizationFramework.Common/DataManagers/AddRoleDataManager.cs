using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Wordprocessing;
using Opteamix.AuthorizationFramework.Common;
using Opteamix.AuthorizationFramework.Common.Interfaces;
using Opteamix.AuthorizationFramework.Data.Model;
using Opteamix.AuthorizationFramework.Database.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opteamix.AuthorizationFramework.Common.DataManagers
{
    public class AddRoleDataManager : IAddRoleDataManager<Role>
    {
        private readonly AuthFrameworkContext context;

        public AddRoleDataManager(AuthFrameworkContext ctx)
        {
            context = ctx;
        }

        public IEnumerable<Role> GetRole(int roleid)
        {
            if (string.IsNullOrEmpty(roleid.ToString()))
            {
                return null;
            }
            else
            {
                var roles = context.Roles.Where(m => m.ApplicationId == roleid && m.IsDeleted == false).Select(m => m);
                return roles;
            }
        }

        public IEnumerable<Role> GetRoleid(Role role)
        {

            var roles = context.Roles.Where(m => m.RoleName == role.RoleName).Select(m => m);
            return roles;

        }

        public IEnumerable<EntityPrivilege> GetEntPrevId(EntityPrivilege entprev)
        {
            var roles = from entities in context.EntityPrivileges
                        where (entities.ModuleId == entprev.ModuleId)
                        where (entities.PrivilegeId == entprev.PrivilegeId)
                        select new EntityPrivilege()
                        {
                            Id = entities.Id,
                            PrivilegeId = entities.PrivilegeId,
                            ModuleId = entities.ModuleId,
                            PermissionId = entities.PermissionId == null ? 0 : entities.PermissionId
                        };
            return roles;

        }

        public string AddRole(Role role)
        {
            if (string.IsNullOrEmpty(role.RoleName) || string.IsNullOrEmpty(role.ShortName) || string.IsNullOrEmpty(role.Id.ToString()))
            {
                return Constants.RequiredFieldError;
            }
            else
            {
                var data = role;
                data.CreatedDate = DateTime.Now;
                data.ModifiedDate = DateTime.Now;
                data.CreatedBy = 1;
                data.ModifiedBy = 1;
                data.IsDeleted = false;

                // Saving Data to Database
                context.Roles.Add(data);
                context.SaveChanges();

                return Constants.RoleAddSuccess;
            }
        }

        public string AddModulePrivilege(EntityPrivilege EntPrev)
        {
            var data = EntPrev;
            context.EntityPrivileges.Add(data);
            context.SaveChanges();
            return Constants.RoleAddSuccess;
            // Saving Data to Database

        }

        public string AddRoleModule(RoleEntity roleEnt)
        {


            var data = roleEnt;

            // Saving Data to Database
            context.RoleEntities.Add(data);
            context.SaveChanges();

            return Constants.RoleAddSuccess;

        }


        public string DeleteRole(List<int> roleid)
        {
            if (string.IsNullOrEmpty(roleid.ToString()))
            {
                return Constants.RequiredFieldError;
            }
            else
            {
                if (roleid.Count() > 0)
                {
                    foreach (int id in roleid)
                    {
                        var role = context.Roles.Where(r => r.Id == id);
                        if (role != null)
                        {
                            context.Roles.Where(r => r.Id == id).ToList().ForEach(val => val.IsDeleted = true);
                            context.SaveChanges();
                        }
                    }
                    return Constants.RoleRemoveSuccess;
                }
                else
                {
                    return Constants.UndefinedError;
                }

            }
        }

        public async Task<bool> UpdateRole(Role role)
        {
            if (string.IsNullOrEmpty(role.RoleName) || string.IsNullOrEmpty(role.ShortName))
            {
                return false;
            }
            else
            {
                try
                {

                    var roleEntities = (from c in context.RoleEntities
                                        where c.RoleId == role.Id
                                        select c);
                    var EnitPreIds = roleEntities.Select(a => a.PermissionprivilegeId).ToArray();
                    context.RoleEntities.RemoveRange(roleEntities);
                    context.SaveChanges();
                    foreach (var item in EnitPreIds)
                    {
                        var name = context.EntityPrivileges.Where(p => p.Id == item).FirstOrDefault();
                        context.EntityPrivileges.Remove(name);
                        context.SaveChanges();
                    }
                    

                    var roleDetail = (from c in context.Roles
                                      where c.Id == role.Id
                                      select c).FirstOrDefault();

                    if (roleDetail != null)
                    {
                        roleDetail.RoleName = role.RoleName;
                        roleDetail.ShortName = role.ShortName;
                        roleDetail.RoleDescription = role.RoleDescription;
                        roleDetail.CreatedDate = DateTime.Now;
                        roleDetail.ModifiedDate = DateTime.Now;
                        context.SaveChanges();
                        return true;

                    }
                }
                catch (Exception ex)
                {
                }
            }
            return true;
        }

        public IEnumerable<RoleEntity> GetRoleEntityByRoleId(int roleId)
        {
            return context.RoleEntities.Where(m => m.RoleId == roleId).Select(m => m);
        }

        public IEnumerable<EntityPrivilege> GetEntityPrivilegeById(int id)
        {
            var roles = from entities in context.EntityPrivileges
                        where (entities.Id == id)
                        select new EntityPrivilege()
                        {
                            Id = entities.Id,
                            PrivilegeId = entities.PrivilegeId,
                            ModuleId = entities.ModuleId==null?0: entities.ModuleId,
                            PermissionId = entities.PermissionId == null ? 0 : entities.PermissionId
                        };
            return roles;
        }

        public  int GetTopEntityValue()
        {
            
            var entity = context.Entities.First();
            if(entity!=null)
            {
                return entity.Id;
            }
            return 0;
        }
    }
}
