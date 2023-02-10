using Opteamix.AuthorizationFramework.Common.Interfaces;
using Opteamix.AuthorizationFramework.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Opteamix.AuthorizationFramework.Database.DTO;

namespace Opteamix.AuthorizationFramework.Common.DataManagers
{
    public class PrivilegeDataManager : IPrivilegeDataManager<Privilege>
    {
        private readonly AuthFrameworkContext context;

        public PrivilegeDataManager(AuthFrameworkContext ctx)
        {
            context = ctx;
        }

        public IEnumerable<FetchPrivilegeDTO> GetPrivilege(int privilegeid)
        {
            if (string.IsNullOrEmpty(privilegeid.ToString()))
            {
                return null;
            }
            else
            {
                var privilege = from privileges in context.Privileges
                                where privileges.Id == privilegeid
                                select new FetchPrivilegeDTO() { PrivilegeId = privileges.Id, PrivilegeName = privileges.PrivilegeName, Description = privileges.Description };
                return privilege;
            }
        }

        public IEnumerable<FetchPrivilegeDTO> GetPrivileges(int appId)
        {
            var privilege = from privileges in context.Privileges 
                            where privileges.IsDeleted == false && privileges.AppId == appId
                            select new FetchPrivilegeDTO() { PrivilegeId = privileges.Id, PrivilegeName = privileges.PrivilegeName, Description = privileges.Description };
            return privilege;
        }

        public string AddPrivilege(Privilege privilege)
        {
            if (string.IsNullOrEmpty(privilege.PrivilegeName))
            {
                return Constants.RequiredFieldError;
            }
            else
            {
                var data = privilege;
                data.CreatedDate = DateTime.Now;
                data.ModifiedDate = DateTime.Now;
                data.IsDeleted=false;
                data.CreatedBy = 1;
                data.ModifiedBy = 1;

                // Saving Data to Database
                context.Privileges.Add(data);
                context.SaveChanges();

                return Constants.PrivilegeAddSuccess;
            }
        }

    public async Task<bool> UpdatePrivilege(Privilege privilege) 
    {
        if (string.IsNullOrEmpty(privilege.PrivilegeName) || string.IsNullOrEmpty(privilege.Id.ToString()))
        {
            return false;
        }
        else 
        {
            try
            {
                var data = (from m in context.Privileges where m.Id == privilege.Id select m).FirstOrDefault();

                if (data != null) 
                {
                    data.PrivilegeName = privilege.PrivilegeName;
                    data.Description = privilege.Description;
                    data.CreatedDate = DateTime.Now;
                    data.ModifiedDate = DateTime.Now;
                    data.CreatedBy = 1;
                    data.ModifiedBy = 1;
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

        public string DeletePrivilege(List<int> privilegeid)
        {
            if (string.IsNullOrEmpty(privilegeid.ToString()))
            {
                return Constants.RequiredFieldError;
            }
            else
            {
                if (privilegeid.Count() > 0)
                {
                    foreach (int id in privilegeid)
                    {
                        var privilege = context.Privileges.Where(m => m.Id == id);
                        if (privilege != null)
                        {
                            context.Privileges.Where(m => m.Id == id).ToList().ForEach(val => val.IsDeleted = true);
                            context.SaveChanges();
                        }
                    }
                    return Constants.PrivilegeRemoveSuccess;
                }
                else
                {
                    return Constants.UndefinedError;
                }
            }
        }
    }
}
