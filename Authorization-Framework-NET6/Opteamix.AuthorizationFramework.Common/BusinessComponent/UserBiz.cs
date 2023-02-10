using Opteamix.AuthorizationFramework.Common.Interface;
using Opteamix.AuthorizationFramework.Data.Model;
using Opteamix.AuthorizationFramework.Database.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opteamix.AuthorizationFramework.Common.BusinessComponent
{
    public class UserBiz:IUserBiz
    {
        IUserDataManager<User> UserRepo;

        public UserBiz(IUserDataManager<User> repo)
        {
            UserRepo = repo;
        }

        public IEnumerable<FetchUserDTO> GetUsers(int clientid)
        {
            return UserRepo.GetUsers(clientid);
        }
    }
}
