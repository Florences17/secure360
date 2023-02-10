using Opteamix.AuthorizationFramework.Data.Model;
using Opteamix.AuthorizationFramework.Database.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opteamix.AuthorizationFramework.Common.Interface
{
    public interface IClientDataManager<T>
    {
        IEnumerable<FetchUpdateClientDTO> GetClient(int clientId);

        IEnumerable<FetchClientDTO> GetClients();
        string AddClient(Client client);

        string DeleteClient(List<int> clientid);

        Task<bool> UpdateClient(Client client);

        Task<Client> ReadClientByName(string shortName);
        Task<Client> Create(string shortName, string name=null);
    }
}
