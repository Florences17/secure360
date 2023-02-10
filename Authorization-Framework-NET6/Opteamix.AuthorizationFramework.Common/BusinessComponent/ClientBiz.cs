using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Opteamix.AuthorizationFramework.Common.Interface;
using Opteamix.AuthorizationFramework.Data.Model;
using Opteamix.AuthorizationFramework.Database.DTO;

namespace Opteamix.AuthorizationFramework.Common.BusinessComponent
{
    public class ClientBiz: IClientsBiz
    {
        IClientDataManager<Client> ClientRepo;
        public ClientBiz(IClientDataManager<Client> repo) => ClientRepo = repo;

        public IEnumerable<FetchClientDTO> GetClients()
        {
            return ClientRepo.GetClients();
        }
        public IEnumerable<FetchUpdateClientDTO> GetClient(int clientid)
        {
            return  ClientRepo.GetClient(clientid);
        }
         public string AddClient(Client client)
        {
            return ClientRepo.AddClient(client);
        }
         public string DeleteClient(List<int> clientid)
        {
            return ClientRepo.DeleteClient(clientid);
        }

        public async Task<bool> UpdateClient (Client client)
        {
            return await ClientRepo.UpdateClient(client);
        }

    }
}
