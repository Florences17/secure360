using Microsoft.EntityFrameworkCore;
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
    public class ApplicationDataManager: IApplicationDataManager<Application>
    {
        private readonly AuthFrameworkContext context;
        public ApplicationDataManager(AuthFrameworkContext ctx)
        {
            context = ctx;
        }

        public IEnumerable<FetchApplicationDTO> GetApplications(int clientid)
        {
            if (string.IsNullOrEmpty(clientid.ToString()))
            {
                return null;
            }
            else
            {
                var modules = from application in context.Applications
                              join client in context.Clients on application.ClientId equals client.Id
                              where client.Id == clientid && application.IsDeleted == false && application.Active == true
                              select new FetchApplicationDTO() { ApplicationId = application.Id, ApplicationName = application.Name, Abbreviation = application.Abbreviation, LogoImage = application.LogoImage, LogoImageName = application.LogoImageName, LogoImageType = application.LogoImageType, Code = application.Code};
                return modules;
            }
        }

         public IEnumerable<FetchApplicationDTO> GetAllApplications()
        {
           

                var AllApps = from application in context.Applications
                             where application.IsDeleted == false 
                              select new FetchApplicationDTO() { ApplicationId = application.Id, ApplicationName = application.Name};
                return AllApps;
            
        }

        public async Task<bool> AddApplication(Application application)
        {
            if (string.IsNullOrEmpty(application.Name) || string.IsNullOrEmpty(application.Code) ||string.IsNullOrEmpty(application.Abbreviation))   
            {
                return false;
            }
            else
            {
                var data = application;
                data.CreatedDate = DateTime.Now;
                data.ModifiedDate = DateTime.Now;
                // Saving Data to Database
                context.Applications.Add(data);
                context.SaveChanges();

                return true;
            }
        }
        public async Task<bool> DeleteApplication( int applicationId)
        {
            if (string.IsNullOrEmpty(applicationId.ToString()))
            {
                return false;
            }
            else
            {
                var module = context.Applications.Where(m => m.Id == applicationId);
                if (module != null)
                {
                    //module.is
                    context.Applications.Where(m => m.Id == applicationId).ToList().ForEach(val => val.IsDeleted = true);
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
        }
        public  IEnumerable<FetchApplicationDTO> GetApplication(int applicationId)
        {
            if (string.IsNullOrEmpty(applicationId.ToString()))
            {
                return null;
            }
            else
            {


                var applicationData = from application in context.Applications
                                      where application.Id == applicationId &&
                                      application.IsDeleted == false
                                      select new FetchApplicationDTO()
                                      {
                                          ApplicationId = application.Id,
                                          ApplicationName = application.Name,
                                          Abbreviation = application.Abbreviation,
                                          Description = application.Description,
                                          Code = application.Code,
                                          LogoImage = application.LogoImage,
                                          LogoImageName = application.LogoImageName,
                                          LogoImageType = application.LogoImageType,
                                          ClientId=application.ClientId
                                      };
                return applicationData;
            }
        }
        public async Task<bool> UpdateApplication(Application application)
        {
            if (string.IsNullOrEmpty(application.Name)  || string.IsNullOrEmpty(application.Abbreviation))
            {
                return false;
            }
            else
            {
                try
                {
                    var module = (from c in context.Applications
                                          where c.Id == application.Id
                                          select c).FirstOrDefault();

                    if (module != null)
                    {
                        module.Name = application.Name;
                        module.Description = application.Description;
                        module.Abbreviation = application.Abbreviation;
                        module.Code = application.Code;
                        module.LogoImage = application.LogoImage;
                        module.LogoImageName = application.LogoImageName;
                        module.LogoImageType = application.LogoImageType;
                        module.ClientId = application.ClientId;

                        module.ModifiedBy = application.ClientId;
                        module.CreatedBy=application.ClientId;
                        module.CreatedDate=application.CreatedDate;
                        module.ModifiedDate = DateTime.Now;
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

            public async Task<Application> ReadApplicationByName(string clientName, string applicationName)
            {
                var result = await context.Applications.Include(x=>x.Client).FirstOrDefaultAsync
                (x=>x.Client.ShortName.Equals(clientName) && x.Abbreviation.Equals(applicationName));

                return result;
            }

            public async Task Delete(string shortName, int ClientId)
            {
                context.Applications.Where(x=>x.Abbreviation.Equals(shortName) && x.ClientId == ClientId).ToList()
                .ForEach(x=>x.IsDeleted = true);
                await context.SaveChangesAsync();
            }

            public async Task<Application> Create(Application app)
            {
                var application = new Application
                {
                    Name = app.Name,
                    Abbreviation = app.Abbreviation,
                    Description = app.Description,
                    Client = app.Client,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now
                };

                context.Applications.Add(application);
                await context.SaveChangesAsync();
                return application;
            }
        }
        }
   

