using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.EntityFrameworkCore;
using Opteamix.AuthorizationFramework.Common.Interface;
using Opteamix.AuthorizationFramework.Data.Model;
using Opteamix.AuthorizationFramework.Database.DTO;

namespace Opteamix.AuthorizationFramework.Common.DataManagers
{
    public class CommonDataManager : ICommonDataManager
    {
        private readonly AuthFrameworkContext _context;
        public CommonDataManager(AuthFrameworkContext context)
        {
            _context = context;
        }

        public async Task<Application> ReadData(string clintId, string applicationId)
        {
            int clintID = Convert.ToInt32(clintId);
            int applicationID = Convert.ToInt32(applicationId);
            var application = await _context.Applications
            .Include(x => x.Client)
            .Include(x => x.Modules)
            .ThenInclude(x => x.Entities)
            .ThenInclude(x => x.EntityPrivileges)
            .ThenInclude(x => x.RoleEntities)
            .Include(x => x.Privileges)
            //.Include(x => x.Roles)
            .FirstOrDefaultAsync(x => x.Id.Equals(applicationID)
            && x.ClientId.Equals(clintID)
            && !x.IsDeleted
            );
            return application;
        }
        //Read entity data only null entry of module data in ModuleId Column of Entity Table
        public async Task<List<EntityRolePrivilege>> ReadEnityDatafor1stLevelwithPrivileges(string clintId, string applicationId)
        {
            int clintID = Convert.ToInt32(clintId);
            int applicationID = Convert.ToInt32(applicationId);
            var entRoleIQ = from app in _context.Applications
                            join Cl in _context.Clients on app.ClientId equals Cl.Id
                            join Pre in _context.Privileges on app.Id equals Pre.AppId
                            join ep in _context.EntityPrivileges on Pre.Id equals ep.PrivilegeId
                            join ent in _context.Entities on ep.PermissionId equals ent.Id
                            where app.Id == applicationID && Cl.Id == clintID && ent.PermissionType == 1
                            select new EntityRolePrivilege()
                            {
                                PrivilegeName = Pre.PrivilegeName,
                                EntityName = ent.DisplayName,
                                EntityShortName = ent.Abbreviation
                            };
            var entRoleLst =await entRoleIQ.ToListAsync();
            return entRoleLst;
        }


        public async Task<Application> ReadDataForRole(string clintId, string applicationId)
        {
            int clintID = Convert.ToInt32(clintId);
            int applicationID = Convert.ToInt32(applicationId);
            var application = await _context.Applications
            .Include(x => x.Client)
            .Include(x => x.Modules)
            .ThenInclude(x => x.Entities)
            .ThenInclude(x => x.EntityPrivileges)
            .ThenInclude(x => x.RoleEntities)
            .Include(x => x.Roles)
            .FirstOrDefaultAsync(x => x.Id.Equals(applicationID)
            && x.ClientId.Equals(clintID)
            && !x.IsDeleted
            );
            return application;
        }


        public async Task<bool> SaveData(DataSet tableData, int? userId = null)
        {
            List<Module> moduleList;
            List<Privilege> privilegeList;
            List<Role> roleList;
            List<Entity> entityList;
            List<EntityPrivilege> entityPrivilegesList;
            List<RoleEntity> roleEntityList;

            if (tableData.Tables.Count > 0)
            {
                moduleList = GetModuleList(tableData.Tables["ModuleTable"], userId);
                privilegeList = GetPrivilegeList(tableData.Tables["PrivilegeTable"], userId);
                roleList = GetRoleList(tableData.Tables["RoleTable"], userId);
                entityList = GetEntityList(tableData.Tables["EntityTable"], moduleList, userId);
                entityPrivilegesList = GetentityPrivilegeList(tableData.Tables["EntityPrivilegeTable"], entityList, privilegeList, userId);
                roleEntityList = GetRoleEntityList(tableData.Tables["RoleEntityTable"], entityPrivilegesList, roleList, userId);
                _context.Modules.AddRange(moduleList);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return false;
                }
                foreach (var module in moduleList)
                {
                    foreach (var entity in entityList)
                    {
                        if (entity.PermissionType == 1)
                        {
                        }
                        if (entity.PermissionType == 2)
                        {
                            if (entity.Abbreviation.Split("_")[1] == module.Abbreviation)
                            {
                                entity.ModuleId = module.Id;
                            }
                        }
                    }
                }
                await _context.SaveChangesAsync();
                _context.Privileges.AddRange(privilegeList);
                await _context.SaveChangesAsync();
                _context.Roles.AddRange(roleList);
                await _context.SaveChangesAsync();
                _context.Entities.AddRange(entityList);
                await _context.SaveChangesAsync();
                _context.EntityPrivileges.AddRange(entityPrivilegesList);
                await _context.SaveChangesAsync();
                _context.RoleEntities.AddRange(roleEntityList);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {

                }
                return true;
            }

            return false;

        }

        private List<Module> GetModuleList(DataTable dataTable, int? userId = null)
        {
            List<Module> moduleList = new List<Module>();

            foreach (DataRow row in dataTable.Rows)
            {
                var module = new Module
                {
                    ApplicationId = Convert.ToInt32(row["ApplicationId"]),
                    Name = row["Name"].ToString().ToUpper(),
                    Description = row["Description"].ToString(),
                    Abbreviation = row["Abbreviation"].ToString().ToUpper(),
                    CreatedBy = userId,
                    ModifiedBy = userId,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    IsDeleted = false
                };

                moduleList.Add(module);
            }

            return moduleList;
        }

        private List<Privilege> GetPrivilegeList(DataTable dataTable, int? userId = null)
        {
            List<Privilege> privilegeList = new List<Privilege>();

            foreach (DataRow row in dataTable.Rows)
            {
                var module = new Privilege
                {
                    PrivilegeName = row["PrivilegeName"].ToString().ToUpper(),
                    PrivilegeType = row["PrivilegeType"].ToString(),
                    DisplayOrder = Convert.ToInt32(row["DisplayOrder"]),
                    AppId = Convert.ToInt32(row["ApplicationId"]),
                    CreatedBy = userId,
                    ModifiedBy = userId,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    IsDeleted = false
                };

                privilegeList.Add(module);
            }

            return privilegeList;
        }

        private List<Role> GetRoleList(DataTable dataTable, int? userId = null)
        {
            List<Role> roleList = new List<Role>();

            foreach (DataRow row in dataTable.Rows)
            {
                var module = new Role
                {
                    RoleName = row["RoleName"].ToString().ToUpper(),
                    ShortName = row["Abbreviation"].ToString().ToUpper(),
                    RoleDescription = row["Description"].ToString(),
                    ApplicationId = Convert.ToInt32(row["Application_Id"]),
                    CreatedBy = userId,
                    ModifiedBy = userId,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    IsDeleted = false
                };

                roleList.Add(module);
            }

            return roleList;
        }

        private List<Entity> GetEntityList(DataTable dataTable, List<Module> moduleList, int? userId = null)
        {
            List<Entity> entityList = new List<Entity>();

            var entity = new Entity
            {
                DisplayName = "Application".ToUpper(),
                Abbreviation = dataTable.Rows[0]["Parent"].ToString().ToUpper(),
                PermissionType = 0,
                Module = null,
                ParentPermission = null
            };

            entityList.Add(entity);

            foreach (DataRow row in dataTable.Rows)
            {
                entity = new Entity
                {
                    DisplayName = row["DisplayName"].ToString().ToUpper(),
                    Abbreviation = row["Prefix"].ToString().ToUpper(),
                    PermissionType = Convert.ToInt32(row["PermissionType"]),
                    Module = moduleList.FirstOrDefault(x => x.Abbreviation.Equals(row["Module"].ToString(), StringComparison.InvariantCultureIgnoreCase)),
                    ParentPermission = entityList.FirstOrDefault(x => x.Abbreviation.Substring(x.Abbreviation.LastIndexOf('_') + 1)
                    .Equals(row["Parent"].ToString(), StringComparison.InvariantCultureIgnoreCase))
                };

                entityList.Add(entity);
            }

            return entityList;
        }

        private List<EntityPrivilege> GetentityPrivilegeList(DataTable dataTable, List<Entity> entitylist, List<Privilege> privilegeList, int? userId = null)
        {
            List<EntityPrivilege> entityPrivilegeList = new List<EntityPrivilege>();

            foreach (DataRow row in dataTable.Rows)
            {
                var entityPrivilege = new EntityPrivilege
                {
                    Permission = entitylist.FirstOrDefault(x => x.Abbreviation.Equals(row["EntityName"].ToString(), StringComparison.InvariantCultureIgnoreCase)),
                    Privilege = privilegeList.FirstOrDefault(x => x.PrivilegeName.Equals(row["PrivilegeName"].ToString(), StringComparison.InvariantCultureIgnoreCase))
                };

                entityPrivilegeList.Add(entityPrivilege);
            }

            return entityPrivilegeList;
        }

        private List<RoleEntity> GetRoleEntityList(DataTable dataTable, List<EntityPrivilege> entityPrivilegeList, List<Role> roleList, int? userId = null)
        {
            List<RoleEntity> roleEntityList = new List<RoleEntity>();

            foreach (DataRow row in dataTable.Rows)
            {
                string entityPrivilegeName = row["EntityPrivilegeName"].ToString();
                int index = entityPrivilegeName.LastIndexOf('_');
                string entity = entityPrivilegeName.Substring(0, index);
                string privilege = entityPrivilegeName.Substring(index + 1);

                var roleEntity = new RoleEntity
                {
                    Role = roleList.FirstOrDefault(x => x.RoleName.Equals(row["RoleName"].ToString(), StringComparison.InvariantCultureIgnoreCase)),
                    Permissionprivilege = entityPrivilegeList.FirstOrDefault(x => x.Permission.Abbreviation.Equals(entity, StringComparison.InvariantCultureIgnoreCase)
                                            && x.Privilege.PrivilegeName.Equals(privilege, StringComparison.InvariantCultureIgnoreCase)),
                    CreatedBy = userId,
                    ModifiedBy = userId,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now
                };

                roleEntityList.Add(roleEntity);
            }

            return roleEntityList;
        }
    }
}