using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Opteamix.AuthorizationFramework.Common.Interface;
using Opteamix.AuthorizationFramework.Data.Model;

namespace Opteamix.AuthorizationFramework.Common.BusinessComponent
{
    public class ExcelBusiness : IExcelBusiness
    {
        private readonly IApplicationDataManager<Application> _applicationData;
        private readonly IClientDataManager<Client> _clientData;
        private readonly ICommonDataManager _commonData;
        List<string>PrivilageList=new List<string>() {"View", "Add", "Edit", "Delete", "Print", "APPROVE", "CONFIRMATION", "PRINT" };
        public ExcelBusiness(IApplicationDataManager<Application> applicationData, IClientDataManager<Client> clientData, ICommonDataManager commonData)
        {
            _applicationData = applicationData;
            _clientData = clientData;
            _commonData = commonData;
        }

        public async Task<bool> ImportExcel(string clientId, string applicationId, IFormFile file, int? userId = null)
        {
            if (string.IsNullOrWhiteSpace(clientId) || string.IsNullOrWhiteSpace(applicationId) || file == null)
            {
                throw new Exception("Null or whiteSpace");
            }
            if (CheckIfExcelFile(file) && file.Length > 0)
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "File");
                await SaveFile(file, path);
                DataSet excelData = ExcelUtility.GetDataFromExcel(Path.Combine(path, file.FileName));
                if (excelData != null)
                {
                    if (!ValidateColumnName(excelData))
                    {
                        throw new Exception("Wrong Column In Excel");
                    }

                    //var application = await _applicationData.ReadApplicationByName(clientName.Trim(), applicationName.Trim());
                    //if (application == null)
                    //{
                    //    var client = await _clientData.ReadClientByName(clientName.Trim());
                    //    if (client == null)
                    //    {
                    //        client = await _clientData.Create(clientName.Trim());
                    //    }
                    //    application = new Application()
                    //    {
                    //        Abbreviation = applicationName,
                    //        Client = client,
                    //        CreatedBy = userId,
                    //        ModifiedBy = userId,
                    //        CreatedDate = DateTime.Now,
                    //        ModifiedDate = DateTime.Now
                    //    };
                    //}
                    //else
                    //{
                    //    await _applicationData.Delete(application.Abbreviation, application.ClientId);
                    //}

                    //application = await _applicationData.Create(application);
                    DataSet tableDate = ConvertData(excelData, Convert.ToInt32(applicationId));
                    var result = await _commonData.SaveData(tableDate, userId);

                    if (result)
                    {
                        return true;
                    }
                    else
                    {
                        throw new Exception("Excel Error");
                    }
                }
                else
                {
                    throw new Exception("Excel Error");
                }
            }
            else
            {
                throw new Exception("Excel Extension Error");
            }
        }

        private bool CheckIfExcelFile(IFormFile file)
        {
            var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
            return (extension == ".xlsx");
        }

        private async Task SaveFile(IFormFile file, string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            using (var stream = new FileStream(Path.Combine(path, file.FileName), FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
        }

        private bool ValidateColumnName(DataSet excelData)
        {
            DataTable firstSheet = excelData.Tables[0];
            DataTable secondSheet = excelData.Tables[1];

            if (firstSheet.Columns[0].ColumnName.Equals("Modules", StringComparison.InvariantCultureIgnoreCase)
                && firstSheet.Columns[1].ColumnName.Equals("Abbreviation", StringComparison.InvariantCultureIgnoreCase)
                 && firstSheet.Columns[2].ColumnName.Equals("Level", StringComparison.InvariantCultureIgnoreCase)
                 && firstSheet.Columns[3].ColumnName.Equals("Parent", StringComparison.InvariantCultureIgnoreCase)
                 && firstSheet.Columns[4].ColumnName.Equals("Prefix", StringComparison.InvariantCultureIgnoreCase)
                 && secondSheet.Columns[0].ColumnName.Equals("Abbreviation", StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }

            return false;
        }

        private DataSet ConvertData(DataSet excelData, int appId)
        {
            DataSet tableData = new DataSet();
            DataTable firstSheet = excelData.Tables[0];
            DataTable moduleTable = CreateModuleTable(firstSheet, appId);
            DataTable entityTable = CreateEntitytable(firstSheet);
            DataTable privilegeTable = CreatePrivilegeTable(firstSheet, appId);
            DataTable entityprivilegeTable = CreateEntityPrivilege(firstSheet);
            DataTable secondSheet = excelData.Tables[1];
            DataTable roletable = CreateRoleTable(secondSheet, appId);
            DataTable roleEnityTable = CreateRoleEntityTable(secondSheet);

            tableData.Tables.Add(moduleTable);
            tableData.Tables.Add(entityTable);
            tableData.Tables.Add(privilegeTable);
            tableData.Tables.Add(entityprivilegeTable);
            tableData.Tables.Add(roletable);
            tableData.Tables.Add(roleEnityTable);

            return tableData;
        }

        private DataTable CreateModuleTable(DataTable dataTable, int appId)
        {
            DataTable moduleTable = new DataTable("ModuleTable");
            moduleTable.Columns.Add("Id", typeof(int));
            moduleTable.Columns.Add("ApplicationId", typeof(int));
            moduleTable.Columns.Add("Name", typeof(string));
            moduleTable.Columns.Add("Description", typeof(string));
            moduleTable.Columns.Add("Abbreviation", typeof(string));
            moduleTable.Columns.Add("IsDeleted",typeof(bool));
            int id = 1;

            foreach (DataRow row in dataTable.Rows)
            {
                if (row["Level"].ToString() == "1")
                {
                    var tempRow = moduleTable.NewRow();
                    tempRow["Id"] = id;
                    tempRow["ApplicationId"] = appId;
                    tempRow["Name"] = row["Modules"].ToString().Trim();
                    tempRow["Description"] = row["Modules"].ToString().Trim();
                    tempRow["Abbreviation"] = row["Abbreviation"].ToString().Trim();
                    tempRow["IsDeleted"] = false;
                    moduleTable.Rows.Add(tempRow);
                    id++;
                }
            }

            if (!ValidateForDuplicate(moduleTable, "Name")
            || !ValidateForDuplicate(moduleTable, "Abbreviation"))
            {
                throw new Exception("Duplicate value in Excel");
            }

            return moduleTable;
        }

        private DataTable CreateEntitytable(DataTable dataTable)
        {
            DataTable entityTable = new DataTable("EntityTable");
            entityTable.Columns.Add("Id", typeof(int));
            entityTable.Columns.Add("DisplayName", typeof(string));
            entityTable.Columns.Add("Abbreviation", typeof(string));
            entityTable.Columns.Add("Module", typeof(string));
            entityTable.Columns.Add("Parent", typeof(string));
            entityTable.Columns.Add("Prefix", typeof(string));
            entityTable.Columns.Add("PermissionType", typeof(string));
            int id = 1;

            foreach (DataRow row in dataTable.Rows)
            {
                var tempRow = entityTable.NewRow();
                tempRow["Id"] = id;
                tempRow["DisplayName"] = row["Modules"].ToString().Trim();
                tempRow["PermissionType"] = row["Level"].ToString().Trim();
                tempRow["Abbreviation"] = row["Abbreviation"].ToString().Trim();
                tempRow["Prefix"] = row["Prefix"].ToString().Trim();
                tempRow["Parent"] = row["Parent"].ToString().Trim();
                string[] tempArray = row["Parent"].ToString().Split('_');
                tempRow["Module"] = tempArray.Length > 1 ? tempArray[1] : "";

                entityTable.Rows.Add(tempRow);
                id++;
            }

            if (!ValidateForBlank(entityTable, "DisplayName", "Abbreviation", "Prefix", "Parent", "PermissionType"))
            {
                throw new Exception("Blank value in Excel");
            }
            else if (!ValidateForDuplicate(entityTable, "DisplayName")
            || !ValidateForDuplicate(entityTable, "Abbreviation")
            || !ValidateForDuplicate(entityTable, "Prefix"))
            {
                throw new Exception("Duplicate values in Excel");
            }
            else if (!ValidateForDuplicate(entityTable, "DisplayName"))
            {
                throw new Exception("Duplicate values in Excel");
            }

            return entityTable;
        }

        private DataTable CreatePrivilegeTable(DataTable dataTable, int appId)
        {
            DataTable privilegeTable = new DataTable("PrivilegeTable");
            privilegeTable.Columns.Add("Id", typeof(int));
            privilegeTable.Columns.Add("PrivilegeName", typeof(string));
            privilegeTable.Columns.Add("PrivilegeType", typeof(string));
            privilegeTable.Columns.Add("DisplayOrder", typeof(int));
            privilegeTable.Columns.Add("ApplicationId", typeof(int));
            privilegeTable.Columns.Add("IsDeleted", typeof(bool));

            int id = 1;

            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                var tempRow = privilegeTable.NewRow();
                tempRow["Id"] = id;
                tempRow["PrivilegeName"] = dataTable.Columns[i].ColumnName.Trim();
                tempRow["PrivilegeType"] = "S";
                tempRow["DisplayOrder"] = id;
                tempRow["ApplicationId"] = appId;
                tempRow["IsDeleted"] = false;

                privilegeTable.Rows.Add(tempRow);
                id++;
            }

            if (!ValidateForBlank(privilegeTable, "PrivilegeName"))
            {
                throw new Exception("Blank Value in Excel");
            }
            else if (!ValidateForDuplicate(privilegeTable, "PrivilegeName"))
            {
                throw new Exception("Duplicate Value in Excel");
            }

            return privilegeTable;
        }

        private DataTable CreateEntityPrivilege(DataTable dataTable)
        {
            DataTable entityPrivilegetable = new DataTable("EntityPrivilegeTable");
            entityPrivilegetable.Columns.Add("Id", typeof(int));
            entityPrivilegetable.Columns.Add("EntityName", typeof(string));
            entityPrivilegetable.Columns.Add("PrivilegeName", typeof(string));
            int id = 1;

            foreach (DataRow row in dataTable.Rows)
            {
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    string colName = dataTable.Columns[i].ColumnName.Trim();
                    if (row[colName].ToString().Trim().Equals("Yes", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var tempRow = entityPrivilegetable.NewRow();
                        tempRow["Id"] = id;
                        tempRow["EntityName"] = row["Prefix"].ToString().Trim();
                        tempRow["PrivilegeName"] = colName;

                        entityPrivilegetable.Rows.Add(tempRow);
                        id++;
                    }
                }
            }

            return entityPrivilegetable;
        }

        private DataTable CreateRoleTable(DataTable dataTable, int appId)
        {
            DataTable roleTable = new DataTable("RoleTable");
            roleTable.Columns.Add("Id", typeof(int));
            roleTable.Columns.Add("RoleName", typeof(string));
            roleTable.Columns.Add("Abbreviation", typeof(string));
            roleTable.Columns.Add("Description", typeof(string));
            roleTable.Columns.Add("Application_Id", typeof(int));
            roleTable.Columns.Add("IsDeleted", typeof(bool));

            DataRow row = dataTable.Rows[0];
            int id = 1;

            for (var i = 0; i < dataTable.Columns.Count; i++)
            {
                var tempRow = roleTable.NewRow();
                tempRow["Id"] = id;
                tempRow["RoleName"] = dataTable.Columns[i].ColumnName.Trim();
                tempRow["Abbreviation"] = row[i].ToString().Trim();
                tempRow["Description"] = dataTable.Columns[i].ColumnName.Trim();
                tempRow["Application_Id"] = appId;
                tempRow["IsDeleted"] = false;

                roleTable.Rows.Add(tempRow);
                id++;
            }

            if (!ValidateForBlank(roleTable, "RoleName", "Abbreviation"))
            {
                throw new Exception("Blank Value in Excel");
            }
            else if (!ValidateForDuplicate(roleTable, "RoleName")
            || !ValidateForDuplicate(roleTable, "Abbreviation"))
            {
                throw new Exception("Duplicate Value in Excel");
            }

            return roleTable;
        }

        private DataTable CreateRoleEntityTable(DataTable dataTable)
        {
            DataTable roleEntityTable = new DataTable("RoleEntityTable");
            roleEntityTable.Columns.Add("Id", typeof(int));
            roleEntityTable.Columns.Add("EntityPrivilegeName", typeof(string));
            roleEntityTable.Columns.Add("RoleName", typeof(string));
           // roleEntityTable.Columns.Add("Abbreviation", typeof(string));

            int id = 0;

            foreach (DataRow row in dataTable.Rows)
            {
                if (id == 0)
                {
                    id++;
                    continue;
                }
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    string colName = dataTable.Columns[i].ColumnName.Trim();
                    if (row[colName].ToString().Trim().Equals("Grant", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var tempRow = roleEntityTable.NewRow();
                        tempRow["Id"] = id;
                        tempRow["EntityPrivilegeName"] = row["Abbreviation"].ToString().Trim();
                        tempRow["RoleName"] = colName;
                        //tempRow["Abbreviation"] = row["Abbreviation"].ToString().Trim();

                        roleEntityTable.Rows.Add(tempRow);
                        id++;

                    }
                }
            }

            if (!ValidateForBlank(roleEntityTable, "EntityPrivilegeName"))
            {
                throw new Exception("Blank Value in Excel");
            }
            //else if (!ValidateForDuplicate(roleEntityTable, "Abbreviation"))
            //{
            //    throw new Exception("Duplicate Value in Excel");
            //}

            return roleEntityTable;
        }

        private bool ValidateForDuplicate(DataTable dataTable, string columnName)
        {
            var distinctData = dataTable.AsEnumerable().Select(x => x[columnName].ToString().ToUpper()).Distinct();
            if (distinctData.Count() == dataTable.Rows.Count)
            {
                return true;
            }
            return false;
        }

        private bool ValidateForBlank(DataTable dataTable, params string[] columnNames)
        {
            foreach (var columnName in columnNames)
            {
                var distinctData = dataTable.AsEnumerable().Select(x => x[columnName].ToString().ToUpper()).Distinct();

                if (dataTable.AsEnumerable().Any(x => string.IsNullOrWhiteSpace(x[columnName].ToString())))
                {
                    return true;
                }
            }
            return true;
        }

        public async Task<MemoryStream> ExportExcel(string clientId, string applicationId)
        {
            if (string.IsNullOrWhiteSpace(clientId) || string.IsNullOrWhiteSpace(applicationId))
            {
                throw new Exception("Null or Blank value");
            }

            var application = await _commonData.ReadData(clientId, applicationId);
            var RoleAppData = await _commonData.ReadDataForRole(clientId, applicationId);
            var EntityData= await _commonData.ReadEnityDatafor1stLevelwithPrivileges(clientId, applicationId);
            if (application == null|| RoleAppData==null)
            {
                throw new Exception("Invalid Application");
            }
            var excelData = CreateExcelData(application, RoleAppData, EntityData);
            string path = Path.Combine(Directory.GetCurrentDirectory(), "File");
            string fileName = "ExportExcel.xlsx";
            string templateFile = "Template\\Template.xlsx";
            ExcelUtility.WriteExcel(fileName, path, excelData, templateFile);
            var memory = new MemoryStream();
            using (var fileStream = new FileStream(Path.Combine(path, fileName), FileMode.Open))
            {
                await fileStream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return memory;

        }

        private DataSet CreateExcelData(Application application,Application RoleAppData,List<EntityRolePrivilege> lstEnity)
        {
            DataTable firstSheet = new DataTable("Privilege");
            firstSheet.Columns.Add("Modules", typeof(string));
            firstSheet.Columns.Add("Abbreviation", typeof(string));
            firstSheet.Columns.Add("Level", typeof(string));
            firstSheet.Columns.Add("Parent", typeof(string));
            firstSheet.Columns.Add("Prefix", typeof(string));
            var privilegeNameList = application.Privileges.Select(x => new { x.PrivilegeName, x.DisplayOrder }).OrderBy(x => x.DisplayOrder).ToList();

            foreach (var privilegeName in privilegeNameList)
            {
                DataColumnCollection columns = firstSheet.Columns;
                if (!columns.Contains(privilegeName.PrivilegeName))
                {
                    firstSheet.Columns.Add(privilegeName.PrivilegeName, typeof(string));
                }               
            }
            var entityNames = lstEnity.Select(a => a.EntityName).Distinct();

            foreach (var entityname in entityNames)
            {
                var tempFirstSheetRowEntity = firstSheet.NewRow();
                tempFirstSheetRowEntity["Modules"] = entityname;
                foreach (var item in lstEnity)
                { 
                    foreach (string name in PrivilageList)
                    {
                        if(entityname==item.EntityName)
                        {
                            tempFirstSheetRowEntity["Abbreviation"] = item.EntityShortName.Split("_")[1];
                            tempFirstSheetRowEntity["Level"] = 1;
                            tempFirstSheetRowEntity["Parent"] = item.EntityShortName.Split("_")[0];
                            tempFirstSheetRowEntity["Prefix"] = item.EntityShortName;
                            if(name.Equals(item.PrivilegeName, StringComparison.InvariantCultureIgnoreCase))
                            {
                                tempFirstSheetRowEntity[name] = "Yes";
                            }
                        }
                     
                    }
                      
                }
                firstSheet.Rows.Add(tempFirstSheetRowEntity);
            }
            var moduleList = application.Modules;
            foreach (var module in moduleList)
            {
            

                foreach (var entity in module.Entities)
                {
                   
                    if (entity.PermissionType > 0)
                    {
                        var tempFirstSheetRow = firstSheet.NewRow();
                        tempFirstSheetRow["Modules"] = entity.DisplayName;
                        tempFirstSheetRow["Abbreviation"] = entity.Abbreviation.Substring(entity.Abbreviation.LastIndexOf('_') + 1);
                        tempFirstSheetRow["Level"] = entity.PermissionType;
                        var tempArray = entity.Abbreviation.Split('_');
                        if(entity.PermissionType==1)
                        {
                           tempFirstSheetRow["Parent"] = tempArray[0];
                        } 
                        if(entity.PermissionType==2)
                        {
                           tempFirstSheetRow["Parent"] = tempArray[1];
                        }
                        //tempFirstSheetRow["Parent"] = tempArray[tempArray.Length - 2];
                        tempFirstSheetRow["Prefix"] = entity.Abbreviation;
                        foreach (var item in entity.EntityPrivileges)
                        {
                            foreach(string name in PrivilageList)
                            {
                                if (item.Privilege.PrivilegeName.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                                {
                                    tempFirstSheetRow[name] = "Yes";
                                }
                            }
                        }
                        //foreach (var privilegeName in privilegeNameList)
                        //{
                        //    tempFirstSheetRow[privilegeName.PrivilegeName] = entity.EntityPrivileges.
                        //    Any(x => x.Privilege.PrivilegeName.Equals(privilegeName.PrivilegeName, StringComparison.InvariantCultureIgnoreCase)) ? "Yes" : "";
                        //}
                        firstSheet.Rows.Add(tempFirstSheetRow);
                    }
                }
            }

            DataTable secondSheet = new DataTable("Role");
            secondSheet.Columns.Add("Abbreviation", typeof(string));
            var roleList = RoleAppData.Roles.ToList();          
            foreach (var role in roleList)
            {
                DataColumnCollection columns = secondSheet.Columns;
                if (!columns.Contains(role.RoleName))
                {
                    secondSheet.Columns.Add(role.RoleName, typeof(string));

                }
            }
            var tempSecondSheetRow = secondSheet.NewRow();
            tempSecondSheetRow["Abbreviation"] = "ShortName";

            foreach (var role in roleList)
            {
                tempSecondSheetRow[role.RoleName] = role.ShortName;
            }

            secondSheet.Rows.Add(tempSecondSheetRow);

            if(RoleAppData.Modules!=null)
            {
                foreach (var module in RoleAppData.Modules)
                {
                    foreach (var entity in module.Entities)
                    {
                        if (entity.PermissionType > 0)
                        {
                            foreach (var entityPrivilege in entity.EntityPrivileges)
                            {
                                tempSecondSheetRow = secondSheet.NewRow();
                                tempSecondSheetRow["Abbreviation"] =
                                entityPrivilege.Permission.Abbreviation + "_" +
                                entityPrivilege.Privilege.PrivilegeName;

                                foreach (var role in roleList)
                                {
                                    if (role.RoleName.Equals("Abbreviation", StringComparison.InvariantCultureIgnoreCase))
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        tempSecondSheetRow[role.RoleName] =
                                                                   entityPrivilege.RoleEntities.Any(x => x.Role.RoleName
                                                                   .Equals(role.RoleName, StringComparison.InvariantCultureIgnoreCase)) ? "Grant" : "";
                                    }

                                }
                                secondSheet.Rows.Add(tempSecondSheetRow);
                            }
                        }
                    }
                }
            } 
            DataSet excelData = new DataSet("ExcelData");
            excelData.Tables.Add(firstSheet);
            excelData.Tables.Add(secondSheet);

            return excelData;
        }
    }
}