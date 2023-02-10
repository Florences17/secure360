using Opteamix.AuthorizationFramework.Common.Interface;
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
    public class ClientDataManager: IClientDataManager<Client>
    {
        private readonly AuthFrameworkContext context;
        public ClientDataManager(AuthFrameworkContext ctx)
        {
            context = ctx;
        }
        public IEnumerable<FetchUpdateClientDTO> GetClient(int clientId)
        {
            if (string.IsNullOrEmpty(clientId.ToString()))
            {
                return null;
            }
            else
            {
                var applications = from application in context.Applications
                                   where application.IsDeleted == false && application.ClientId == clientId
                                   select new FetchApplicationDTO()
                                   {
                                       ApplicationId = application.Id,
                                       ApplicationName = application.Name,
                                       Abbreviation = application.Abbreviation,
                                       LogoImage = application.LogoImage,
                                       LogoImageName = application.LogoImageName,
                                       LogoImageType = application.LogoImageType
                                   };

                var client = from clients in context.Clients
                             where clients.IsDeleted == false && clients.Id == clientId
                             select new FetchUpdateClientDTO()
                             {
                                 ClientId = clients.Id,
                                 ClientName = clients.Name,
                                 ShortName = clients.ShortName,
                                 WebsiteAddress = clients.WebsiteAddress,
                                 EmailAddress = clients.EmailAddress,
                                 AddressLine1 = clients.AddressLine1,
                                 AddressLine2 = clients.AddressLine2,
                                 City = clients.City,
                                 ContactPerson = clients.ContactPerson,
                                 ContactPersonEmailAddress = clients.ContactPersonEmailAddress,
                                 ContactPersonPhoneNumber = clients.ContactPersonPhoneNumber,
                                 ClientLogoImage = clients.ClientLogoImage,
                                 ClientLogoName = clients.ClientLogoName,
                                 ClientLogoImageType = clients.ClientLogoImageType,
                                 Applications = applications.ToList()
                             };

                return client;
            }
        }

        public IEnumerable<FetchClientDTO> GetClients()
        {
            var client = from clients in context.Clients where clients.IsDeleted == false select new FetchClientDTO 
            { 
                ClientId = clients.Id, 
                ClientName = clients.Name, 
                ShortName = clients.ShortName, 
                ContactPerson = clients.ContactPerson, 
                LogoImage = clients.ClientLogoImage, 
                ClientLogoImageType = clients.ClientLogoImageType 
            };

            return client;
        }

        public string AddClient(Client client)
        {
            if (string.IsNullOrEmpty(client.Name) || string.IsNullOrEmpty(client.ShortName) || string.IsNullOrEmpty(client.ContactPerson))
            {
                return Constants.RequiredFieldError;
            }
            else
            {
                var data = client;
                data.CreatedDate = DateTime.Now;
                data.ModifiedDate = DateTime.Now;

                // Saving Data to Database
                context.Clients.Add(data);
                context.SaveChanges();

                return Constants.ClientAddSuccess;
            }
        }


        public string DeleteClient(List<int> clientid)
        {
            if (string.IsNullOrEmpty(clientid.ToString()))
            {
                return Constants.RequiredFieldError;
            }
            else
            {
                if (clientid.Count() > 0)
                {
                    foreach(int id in clientid)
                    {
                        var client = context.Clients.Where(c => c.Id == id);
                        if (client != null)
                        {
                            context.Clients.Where(c => c.Id == id).ToList().ForEach(val => val.IsDeleted = true);
                            context.SaveChanges();
                            
                        }
                    }
                    return Constants.ClientRemoveSuccess;
                }
                else{
                     return Constants.UndefinedError;
                }
               
            }
        }


        public async Task<bool> UpdateClient(Client client)
        {
            if (string.IsNullOrEmpty(client.Name) || string.IsNullOrEmpty(client.ShortName))
            {
                return false;
            }
            else
            {
                try
                {
                    var data = (from c in context.Clients
                                          where c.Id == client.Id
                                          select c).FirstOrDefault();

                    if (data != null)
                    {
                        data.Name = client.Name;
                        data.ShortName = client.ShortName;
                        data.ClientStatus = 1;
                        data.IsDeleted = false;
                        data.ClientLogoImage = client.ClientLogoImage;
                        data.ClientLogoName = client.ClientLogoName;
                        data.ClientLogoImageType = client.ClientLogoImageType;
                        data.EmailAddress = client.EmailAddress;
                        data.WebsiteAddress = client.WebsiteAddress;
                        data.AddressLine1 = client.AddressLine1;
                        data.AddressLine2 = client.AddressLine2;
                        data.ContactPerson = client.ContactPerson;
                        data.ContactPersonEmailAddress = client.ContactPersonEmailAddress;
                        data.ContactPersonPhoneNumber = client.ContactPersonPhoneNumber;
                        data.City = client.City;
                        data.CreatedDate = DateTime.Now;
                        data.ModifiedDate = DateTime.Now;
                        //data.CreatedBy = 1;
                       // data.ModifiedBy = 1;
                        //data.IsDeleted = false;
                        context.SaveChanges();
                        return true;

                    }
                }
                catch (Exception ex)
                {
                }
            }
                return true;
        }

        public async Task<Client> ReadClientByName(string shortName)
        {
            var client = await context.Clients.FirstOrDefaultAsync(x=>x.ShortName.Equals(shortName));

            return client;
        }

        public async Task<Client> Create(string shortName, string name=null)
        {
            var client = new Client
            {
                Name = shortName,
                ShortName = shortName,                
                ClientStatus = 1,
                IsDeleted = false,
                ClientLogoImage = null,
                ClientLogoName = null,
                ClientLogoImageType = null,
                EmailAddress = null,
                WebsiteAddress = null,
                AddressLine1 = null,
                AddressLine2 = null,
                City = null,
                ContactPerson = null,
                ContactPersonEmailAddress = null,
                ContactPersonPhoneNumber = null

            };
            try
            {
                var data = client;
                data.CreatedDate = DateTime.Now;
                data.ModifiedDate = DateTime.Now;

                // Saving Data to Database
                context.Clients.Add(data);
                context.SaveChanges();
            }
            catch (Exception ex)
            { }
             return client;
        }
    }
}
