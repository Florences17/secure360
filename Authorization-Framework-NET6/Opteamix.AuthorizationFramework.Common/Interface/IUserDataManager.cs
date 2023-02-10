using Opteamix.AuthorizationFramework.Database.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opteamix.AuthorizationFramework.Common.Interface
{
    public interface IUserDataManager<T>
    {
        IEnumerable<FetchUserDTO> GetUsers(int clientId);
    }
}
