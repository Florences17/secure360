using Microsoft.EntityFrameworkCore;
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
    public class ModulesDataManager : IModulesDataManager<Module>
    {
        private readonly AuthFrameworkContext context;

        public ModulesDataManager(AuthFrameworkContext ctx)
        {
            context = ctx;
        }

        public IEnumerable<FetchModuleDTO> GetModules(int appid)
        {
            if (string.IsNullOrEmpty(appid.ToString()))
            {
                return null;
            }
            else
            {
                var modules = from module in context.Modules
                              join application in context.Applications on module.ApplicationId equals application.Id
                              where module.ApplicationId == appid && module.IsDeleted == false
                              select new FetchModuleDTO() {ModuleId=module.Id, ModuleName=module.Name, Abbreviation=module.Abbreviation, Description=module.Description, ApplicationName=module.App.Name };
                return modules;
            }
        }

        public IEnumerable<FetchModuleDTO> GetModule(int moduleid)
        {
            if (string.IsNullOrEmpty(moduleid.ToString()))
            {
                return null;
            }
            else
            {
                var modules = from module in context.Modules
                              join application in context.Applications on module.ApplicationId equals application.Id
                              where module.Id == moduleid && module.IsDeleted == false
                              select new FetchModuleDTO() { ModuleId = module.Id, ModuleName = module.Name, Abbreviation = module.Abbreviation, Description = module.Description, ApplicationName = module.App.Name };
                return modules;
            }
        }

        public IEnumerable<EntityRolePrivilege> GetEntityRolePrivileges(string clientName, string applicationName)
        {

            if (string.IsNullOrEmpty(clientName.ToString()))
            {
                return null;
            }
            else
            {
                //var entRoleLst = from app in context.Applications
                //              join Cl in context.Clients on app.ClientId equals Cl.Id
                //              join role in context.Roles on app.Id equals role.ApplicationId
                //              join Pre in context.Privileges on app.Id equals Pre.AppId   
                //              join mod in context.Modules on app.Id equals mod.ApplicationId
                //              join ent in context.Entities on mod.Id equals ent.ModuleId
                //              where app.Name == applicationName && Cl.Name == clientName
                //              select new EntityRolePrivilege()
                //              { ClientShortName = Cl.ShortName, ApplicationShortName = app.Name
                //              ,RoleName=role.RoleName,RoleShortName=role.ShortName,
                //              PrivilegeName=Pre.PrivilegeName,
                //              EntityName=ent.DisplayName,
                //              EntityShortName=ent.Abbreviation                              
                //              }; 
                var entRoleLst = from app in context.Applications
                              join Cl in context.Clients on app.ClientId equals Cl.Id
                              join role in context.Roles on app.Id equals role.ApplicationId
                              join Pre in context.Privileges on app.Id equals Pre.AppId   
                              join re in context.RoleEntities on role.Id equals re.RoleId 
                              join ep in context.EntityPrivileges on re.PermissionprivilegeId equals  ep.Id
                              join ent in context.Entities on ep.PermissionId equals ent.Id
                              where app.Name == applicationName && Cl.Name == clientName && ep.PrivilegeId==Pre.Id
                              select new EntityRolePrivilege()
                              { ClientShortName = Cl.ShortName, ApplicationShortName = app.Name
                              ,RoleName=role.RoleName,RoleShortName=role.ShortName,
                              PrivilegeName=Pre.PrivilegeName,
                              EntityName=ent.DisplayName,
                              EntityShortName=ent.Abbreviation                              
                              };
                int lenght = entRoleLst.Count();
                return entRoleLst;
            }
        }

        public IEnumerable<FetchSubModuleDTO> GetSubModules()
        {
            var submodules = from entities in context.Entities
                                select new FetchSubModuleDTO() { SubModuleId = entities.Id, SubModuleName = entities.DisplayName,
                                    Abbreviation = entities.Abbreviation, ModuleId = entities.ModuleId==null?0:entities.ModuleId };
            return submodules;
        }

        public IEnumerable<FetchSubModuleDTO> GetSubModules(int moduleid)
        {
            if (string.IsNullOrEmpty(moduleid.ToString()))
            {
                return null;
            }
            else
            {
                var submodules = from entities in context.Entities
                              where entities.ModuleId == moduleid
                              select new FetchSubModuleDTO() { SubModuleId = entities.Id, SubModuleName = entities.DisplayName, Abbreviation = entities.Abbreviation, ModuleId = entities.ModuleId };
                return submodules;
            }
        }

        public IEnumerable<FetchSubModuleDTO> GetL2SubModules(int submoduleid)
        {
            if (string.IsNullOrEmpty(submoduleid.ToString()))
            {
                return null;
            }
            else
            {
                var submodules = from entities in context.Entities
                                 where entities.ParentPermissionId == submoduleid
                                 select new FetchSubModuleDTO() { SubModuleId = entities.Id, SubModuleName = entities.DisplayName, Abbreviation = entities.Abbreviation, ParentModuleId = entities.ParentPermissionId };
                return submodules;
            }
        }

        public IEnumerable<FetchSubModuleDTO> GetSubModule(int submoduleid)
        {
            if (string.IsNullOrEmpty(submoduleid.ToString()))
            {
                return null;
            }
            else
            {
                var submodules = from entities in context.Entities
                                 where entities.Id == submoduleid
                                 select new FetchSubModuleDTO() { SubModuleId = entities.Id, SubModuleName = entities.DisplayName, Abbreviation = entities.Abbreviation, ModuleId = entities.ModuleId, ParentModuleId = entities.ParentPermissionId };
                return submodules;
            }
        }

        public string AddModule(Module module)
        {
            if (string.IsNullOrEmpty(module.Name) || string.IsNullOrEmpty(module.Abbreviation) || string.IsNullOrEmpty(module.ApplicationId.ToString()))
            {
                return Constants.RequiredFieldError;
            }
            else
            {
                var data = module;
                data.CreatedDate = DateTime.Now;
                data.ModifiedDate = DateTime.Now;
                data.CreatedBy = 1;
                data.ModifiedBy = 1;
                data.IsDeleted = false;

                // Saving Data to Database
                context.Modules.Add(data);
                context.SaveChanges();

                return Constants.ModuleAddSuccess;
            }
        }

        public string AddSubModule(Entity entity)
        {
            if (string.IsNullOrEmpty(entity.DisplayName) || string.IsNullOrEmpty(entity.Abbreviation) || string.IsNullOrEmpty(entity.ModuleId.ToString()))
            {
                return Constants.RequiredFieldError;
            }
            else 
            {
                var data = entity;
                context.Entities.Add(data);
                context.SaveChanges();
                return Constants.ModuleAddSuccess;
            }
        }

        public string AddL2SubModule(Entity entity)
        {
            if (string.IsNullOrEmpty(entity.DisplayName) || string.IsNullOrEmpty(entity.Abbreviation) || string.IsNullOrEmpty(entity.ParentPermissionId.ToString()))
            {
                return Constants.RequiredFieldError;
            }
            else
            {
                var data = entity;
                context.Entities.Add(data);
                context.SaveChanges();
                return Constants.ModuleAddSuccess;
            }
        }

        public async Task<bool> UpdateModule(Module module) 
        {
            if (string.IsNullOrEmpty(module.Name) || string.IsNullOrEmpty(module.Abbreviation) || string.IsNullOrEmpty(module.Id.ToString()))
            {
                return false;
            }
            else 
            {
                try
                {
                    var data = (from m in context.Modules where m.Id == module.Id select m).FirstOrDefault();

                    if (data != null) 
                    {
                        data.Name = module.Name;
                        data.Abbreviation = module.Abbreviation;
                        data.Description = module.Description;
                        data.CreatedDate = DateTime.Now;
                        data.ModifiedDate = DateTime.Now;
                        data.CreatedBy = 1;
                        data.ModifiedBy = 1;
                        data.ApplicationId= module.ApplicationId;
                        data.IsDeleted = false;

                        context.SaveChanges();
                        return true;
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return true;
        }

        public async Task<bool> UpdateSubModule(Entity entity)
        {
            if (string.IsNullOrEmpty(entity.DisplayName) || string.IsNullOrEmpty(entity.Abbreviation) || string.IsNullOrEmpty(entity.ModuleId.ToString()))
            {
                return false;
            }
            else
            {
                try
                {
                    var data = (from e in context.Entities where e.Id == entity.Id select e).FirstOrDefault();

                    if (data != null)
                    {
                        data.DisplayName = entity.DisplayName;
                        data.Abbreviation = entity.Abbreviation;
                        data.ModuleId = entity.ModuleId;
                        data.ParentPermissionId = entity.ParentPermissionId;
                        context.SaveChanges();
                        return true;
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return true;
        }

        public async Task<bool> UpdateL2SubModule(Entity entity)
        {
            if (string.IsNullOrEmpty(entity.DisplayName) || string.IsNullOrEmpty(entity.Abbreviation) || string.IsNullOrEmpty(entity.ParentPermissionId.ToString()))
            {
                return false;
            }
            else
            {
                try
                {
                    var data = (from e in context.Entities where e.Id == entity.Id select e).FirstOrDefault();

                    if (data != null)
                    {
                        data.DisplayName = entity.DisplayName;
                        data.Abbreviation = entity.Abbreviation;
                        data.ModuleId = entity.ModuleId;
                        data.ParentPermissionId = entity.ParentPermissionId;
                        context.SaveChanges();
                        return true;
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return true;
        }

        public string DeleteModule(List<int> moduleid)
        {
            if (string.IsNullOrEmpty(moduleid.ToString()))
            {
                return Constants.RequiredFieldError;
            }
            else
            {
                if (moduleid.Count() > 0)
                {
                    foreach (int id in moduleid)
                    {
                        var module = context.Modules.Where(m => m.Id == id);
                        if (module != null)
                        {
                            context.Modules.Where(m => m.Id == id).ToList().ForEach(val => val.IsDeleted = true);
                            context.SaveChanges();
                        }
                    }
                    return Constants.ModuleRemoveSuccess;
                }
                else 
                {
                    return Constants.UndefinedError;
                }
                
            }
        }
    }
}
