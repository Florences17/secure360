using Opteamix.AuthorizationFramework.Database.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opteamix.AuthorizationFramework.Common.Interface
{
    public interface IUserBiz
    {
        IEnumerable<FetchUserDTO> GetUsers(int clientid);
    }
}
