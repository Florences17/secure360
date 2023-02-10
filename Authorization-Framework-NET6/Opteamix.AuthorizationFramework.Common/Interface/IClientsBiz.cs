using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Opteamix.AuthorizationFramework.Data.Model;
using Opteamix.AuthorizationFramework.Database.DTO;

namespace Opteamix.AuthorizationFramework.Common.Interface
{
    public interface IClientsBiz
    {
        IEnumerable<FetchUpdateClientDTO> GetClient(int clientId);

        IEnumerable<FetchClientDTO> GetClients();

        string AddClient(Client client);

        string DeleteClient(List<int> clientid);
        Task<bool> UpdateClient(Client client);
    }
}
