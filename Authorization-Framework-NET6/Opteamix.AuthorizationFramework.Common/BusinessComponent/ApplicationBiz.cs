using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Opteamix.AuthorizationFramework.Common.Interface;
using Opteamix.AuthorizationFramework.Common.Interfaces;
using Opteamix.AuthorizationFramework.Data.Model;
using Opteamix.AuthorizationFramework.Database.DTO;

namespace Opteamix.AuthorizationFramework.Common.BusinessComponent
{
    public class ApplicationBiz : IApplicationBiz
    {
        IApplicationDataManager<Application> ApplicationRepo;
        public ApplicationBiz(IApplicationDataManager<Application> repo)
        {
            ApplicationRepo = repo;

        }
        public IEnumerable<FetchApplicationDTO> GetApplications(int clientid)
        {
            return ApplicationRepo.GetApplications(clientid);
        }
        public IEnumerable<FetchApplicationDTO> GetAllApplications()
        {
            return ApplicationRepo.GetAllApplications();
        }
        public async Task<bool> AddApplication(Application application)
        {
            return await ApplicationRepo.AddApplication(application);
        }
        public async Task<bool> DeleteApplication(int applicationId)
        {
            return await ApplicationRepo.DeleteApplication(applicationId);
        }
        public  IEnumerable<FetchApplicationDTO> GetApplication(int applicationId)
        {
            return  ApplicationRepo.GetApplication(applicationId);
        }
        public async Task<bool> UpdateApplication(Application application)
        {
            return await ApplicationRepo.UpdateApplication(application);
        }

    }
}
