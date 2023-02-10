using Opteamix.AuthorizationFramework.Common.Interface;
using Opteamix.AuthorizationFramework.Data.Model;
using Opteamix.AuthorizationFramework.Database.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opteamix.AuthorizationFramework.Common.DataManagers
{
    public class UserDataManager: IUserDataManager<User>
    {
        private readonly AuthFrameworkContext context;

        public UserDataManager(AuthFrameworkContext ctx)
        {
            context = ctx;
        }

        public IEnumerable<FetchUserDTO> GetUsers(int clientId)
        {
            if (string.IsNullOrEmpty(clientId.ToString()))
            {
                return null;
            }
            else
            {
                IEnumerable<FetchUserDTO> UserList = from manager in context.User 
                                                     join users in context.User on manager.Id equals users.ManagerId
                                                     join roles in context.Roles on users.RoleId equals roles.Id
                                                     where users.IsDeleted == false && users.ClientId == clientId
                                                     select new FetchUserDTO()
                                                     {
                                                         Id = users.Id,
                                                         FirstName = users.FirstName,
                                                         LastName = users.LastName,
                                                         FullName = users.FullName,
                                                         UserName = users.UserName,
                                                         Email = users.Email,
                                                         ManagerId = users.ManagerId,
                                                         ManagerName = manager.FullName,
                                                         RoleId = users.RoleId,
                                                         Role = roles.RoleName,
                                                         Status = "Active"
                                                     };
                return UserList;
            }
        }
    }
}
