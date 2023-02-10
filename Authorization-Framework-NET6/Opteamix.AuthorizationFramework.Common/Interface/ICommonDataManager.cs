using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Opteamix.AuthorizationFramework.Data.Model;

namespace Opteamix.AuthorizationFramework.Common.Interface
{
    public interface ICommonDataManager
    {
        Task<Application> ReadData(string clientName, string applicationName);
        Task<bool> SaveData(DataSet tableData, int? userId = null);
        Task<Application> ReadDataForRole(string clintId, string applicationId);
        Task<List<EntityRolePrivilege>> ReadEnityDatafor1stLevelwithPrivileges(string clintId, string applicationId);
    }
}